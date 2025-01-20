# .NET Labb3 - Quiz Configurator (nov 2024)
This is the third submitted project made through my education as a .NET developer in the C# course.

The purpose of this project is to build an application to configure quiz packages with usercreated questions, and run quiz rounds. The app should be written in WPF and XAML, and built on Model-View-ViewModel (MVVM) architecture. MVVM is an architectural design pattern in software development, which facilitates the separation of the development of a graphical user interface from the underlying back-end logic. Since the time for the project is limited, parts of the basic structure were given out through code alongs.

**To fetch this version of the project, copy the repository from the submitted commit on the 11th of november, 2024.**

# Update: Databases Labb3 - Quiz Configurator with MongoDB (jan 2025)

As another assignment we've had the choice to further develop an existing project. In this lab, we develop an application in C# that uses MongoDB.Driver and/or Entity Framework to allow the user to read and update data in a MongoDB database. The application should not require any existing database. It should connect to MongoDB on localhost and create a database itself and populate this with any demo data (if it does not already exist). Important: The name of the database that is created must be your first and last name (Example: "SvenSvensson"). The app must then use the existing database in future runs.

Update the Quiz Application from last course in C# so that questionpacks including questions are stored in MongoDB instead of in json files. In addition to the information (name, difficulty, timelimit) that is already available, you must also be able to select a category on each questionpack from a dropdown with predetermined categories. The predetermined categories must be stored in a separate collection in MongoDB and there must be functionality in the app to add and remove categories from it.
