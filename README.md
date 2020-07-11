# Trading App
This is a timed assignment and the efforts been put on this excercise are 75 minutes. It could have been much better if more time would have spent on the design and architectural choices for this application.

## Implementation Notes ##
* Logging - Added time stamped log file. This is generated by Serilog and is being generated in the logs folder
* Dependency injection - Implemented the autofac to introduce the IoC and decouple the different fucntionalities.
* Testability - Nearly all the services are backed by interfaces and hence are fully testable.
* Validations - Though no real refactoring has been done on the validation fo the data, a new check has been added as requested.
* Sample Trade Data - I have created a sample text file consisting of some arbitrary data and kept it in the "SampleDataFiles" folder. To run this processor I took liberty to add some basic file reading logic in the Main function.
* app config - Added configuration builder to read app settings form json file. connection string has been placed in it.

## Recommendations ##
* Test Coverage - Add unit test coverage to a maximum possible level
* Layered Architecture - Easy split of the infra, db and core layers possible
* Integration Testing - An end to end testing with actual result validations would be much more valuable in such finanical processings.
* Serilog config - A little change could move the logger configuration to the app settings json file and reduce dependency on direct code changes in order to make changes to log file name or location etc.
* Reporting
* Serilog Portal - Rather than logging onto a file/console, a better choice is to spit out all the logs on to a highly available portal whcih could be access 24/7.
* Alerts - Some PUSH mechanism must be there to send alerts to stakeholders if the processor encounters any issues during run. This could be acheived via using Twilio for text messages or emails, using pubnub to send liv epush messages to some other portals or mobile devices etc.
