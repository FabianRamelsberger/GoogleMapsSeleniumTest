# Google Maps UI Testing
## Overview
This project aims to automate the testing of Google Maps using search terms in various languages and browsers. It's designed to ensure the robustness and flexibility of Google Maps functionalities across different platforms. It was used for a home assignment for an application for the company Wooga.
### Features
Multi-language Support: Tests are currently available in English and German.
Cross-browser Compatibility: Runs on Chrome and Firefox.
Test Cases: Includes 5 test cases covering various edge cases.
Modular Testing: Utilizes annotations on methods with a single test per method for clarity and maintenance ease.
### Possible Extensions
Additional Locations: More geographic locations can be added to enhance testing coverage.
Multi-Class Testing: Organize tests into multiple classes for better structure.
Language Expansion: Include more languages to ensure wider applicability.
Browser Support: Extend tests to include Edge and Safari for comprehensive browser compatibility.
Result Validation: Enhance tests to verify search terms within the results, not just in the URL.
Dynamic Searching: Implement dynamic searching within JSON result lists for improved test accuracy.
### Technology Stack
Languages: C#
UI Technologies: Selenium
Test Framework: NUnit
Continuous Integration: Jenkins
Development Environment: Rider IDE

## Setup and Running Tests
### Test Devices
Windows 11 on a laptop or macOS Sonoma 14.4.1 on a Mac
### Running Tests
Ensure that the NUnit framework is set up in your development environment. Execute the tests using the standard NUnit test running procedures in your IDE or through a command line interface.
#### CI Integration
The project was integrated with a local Jenkins for continuous testing. 
#### Test Results
All tests are logged and detailed in the Jenkins output, ensuring traceability and immediate feedback on failures.
Passed! Errors: 0, successful: 22, skipped: 0, total: 22, duration: 3 m 23 s - WoogaTakeHome.dll (net8.0)

# Documentation
Further documentation includes code comments, usage instructions, and a detailed test case specification. This documentation is crucial for maintenance and future development.
## Submission Notes
This project uses C#, Selenium, and NUnit. The choice of these technologies was based on previous experience and their robustness for UI testing frameworks.
