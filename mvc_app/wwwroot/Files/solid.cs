public class Employee   	//Single responsibility principle
							//only handling one responsibility (report generation and employee class handles
							//only one thing)
{
	public int Employee_Id;
	public string Employee_Name;

	public bool InsertIntoEmployeeTable(Employee em)
	{
		System.Console.Writeline("value inserted into employee table");
		return true;
	}
	
}


public class IReportGeneration
{
	public virtual void GenerateReport(Employee em)//functionality implemented open closed principle
	{
	}
}
public class CrystalReportGeneraion : IReportGeneration//any new functionality can be extended without modifiying the underlying class
{
	public override void GenerateReport(Employee em)
	{
		System.Console.Writeline("Crystal report generated");
	}
}
public class PDFReportGeneraion : IReportGeneration
{
	public override void GenerateReport(Employee em)
	{
		System.Console.Writeline("PDF report generated");
	}
}
	
	
public interface IAddOperation//interface segregation so no class should have to use an interface functionality that is irrelevant to it
{
	bool AddEmployeeDetails();//if a class wants to both add details and get details it may implement both 
								// but if it wants to do one or the other it should not have to implement both
}
public interface IGetOperation
{
	bool ShowEmployeeDetails(int employeeId);
}