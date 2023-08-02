# Test-Automation


This is a C#, NUnit, Specflow project that follows the BDD (Behavior-Driven Development) approach. It utilizes Specflow and other libraries to facilitate the creation and execution of automated tests.

Prerequisites To run this project, you need to have the following software installed on your machine:
-> dotnet 6 or higher
-> Your preferred IDE (e.g., Visual Studio or VS Code)


Getting Started:
1. Clone this repository to your local machine.
2. Open the project in Visual Studio or VS Code.
3. Clean and Build the project.
4. Open the command line and run: dotnet test


Note* By default, this project is set up to run on BrowserStack. So, if you would like to run it on BrowserStack, you need to update your username and AccessKey in the following classes:
1. AutomationFramework.TestProject.Driver.DriverManager (Line #76, #77, #95 and #96)
2. AutomationFramework.TestProject.Hooks.WebHooks (Line#76)

Writing and Running Tests The tests in this project are written using Specflow and Gherkin syntax. The feature files can be found in the ..\AutomationFramework\TestProject\ExampleSite\Features\ directory. Add a new feature file following the Gherkin language to create new tests.

Specflow - The BDD framework used in this project.
Nuget - Dependency management.
