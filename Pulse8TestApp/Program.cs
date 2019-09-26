using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse8TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string testString;
                Console.Write("\nEnter Member ID:");
                testString = Console.ReadLine();

                using (var context = new Pulse8TestDBEntities())
                {
                    var memberCategories = context.Database.SqlQuery<MemberCategory>(
                        @"SELECT * FROM (
                            SELECT 
	                            m.MemberID
	                            ,m.FirstName
	                            ,m.LastName
	                            ,d.DiagnosisID as MostSevereDiagnosisID
	                            ,d.DiagnosisDescription as MostSevereDiagnosisDescription
	                            ,dc.DiagnosisCategoryID
	                            ,dc.CategoryDescription
	                            ,dc.CategoryScore
	                            ,CASE WHEN dc.DiagnosisCategoryID IS NULL THEN 1 ELSE 
		                            CASE WHEN ROW_NUMBER() OVER (PARTITION BY m.MemberId ORDER BY dc.DiagnosisCategoryID ASC) = 1 THEN 1 ELSE 0 END
	                            END AS IsMostSevereCategory
	                            ,ROW_NUMBER() OVER (PARTITION BY m.MemberId, dc.DiagnosisCategoryID ORDER BY d.DiagnosisID ASC) as rownum
                            FROM Member m
	                            LEFT JOIN MemberDiagnosis md ON m.MemberID = md.MemberID
	                            LEFT JOIN DiagnosisCategoryMap dcm ON dcm.DiagnosisID = md.DiagnosisID
	                            LEFT JOIN DiagnosisCategory dc ON dc.DiagnosisCategoryID = dcm.DiagnosisCategoryID
	                            LEFT JOIN Diagnosis d ON d.DiagnosisID = md.DiagnosisID
                            GROUP  BY m.MemberID, 
	                            m.FirstName, 
	                            m.LastName, 
	                            dc.DiagnosisCategoryID, 
	                            dc.CategoryDescription, 
	                            dc.CategoryScore, 
	                            d.DiagnosisID, 
	                            d.DiagnosisDescription) t 
                            WHERE  t.rownum = 1
                            AND t.MemberID = {0}", testString
                            ).ToList();

                    Console.WriteLine($"\nMemberID | " +
                                      $"FirstName | " +
                                      $"LastName | " +
                                      $"MostSevere | " +
                                      $"MostSevere       | " +
                                      $"Diagnosis  | " +
                                      $"Category    | " +
                                      $"Category | " +
                                      $"IsMostSevere");

                    Console.WriteLine($"         | " +
                                      $"          | " +
                                      $"         | " +
                                      $"DiagID     | " +
                                      $"DiagDesc         | " +
                                      $"CategoryID | " +
                                      $"Description | " +
                                      $"Score    | " +
                                      $"Category");

                    Console.WriteLine($"---------+-----------+----------+------------+------------------+----------------+---------+----------+-------------");

                    foreach (var member in memberCategories)
                    {
                        Console.WriteLine($"{member.MemberID,8} | " +
                                          $"{member.FirstName,9} | " +
                                          $"{member.LastName,8} | " +
                                          $"{member.MostSevereDiagnosisID,10} | " +
                                          $"{member.MostSevereDiagnosisDescription,16} | " +
                                          $"{member.DiagnosisCategoryID,10} | " +
                                          $"{member.CategoryDescription,11} | " +
                                          $"{member.CategoryScore,8} | " +
                                          $"{member.IsMostSevereCategory,12}");
                    }
                }
            }
        }
    }
}
