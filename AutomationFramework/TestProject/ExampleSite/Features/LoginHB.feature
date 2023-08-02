Feature: Login functionality for testproject portal

@login @Regression @Smoke @testproject @UI
Scenario Outline: Verify that user is able to login using valid credentials
	Given I am at home page of testproject portal
	When I enter a valid <username> and <password>
	And I click the LogIn button

Examples:
	| TestCaseId | username | password |
	| 1          | test123  | 12345    |




