Project Description
============
Program designed for users to enter in a matrix, and it will reduce that matrix to Reduced Row Echelon Form, and output a LaTeX file that shows the steps.

This github repo is a fork of the one hosted at [codeplex](https://matrixreducer.codeplex.com/). The one on codeplex is no longer maintained. 

Use
-----
In order to use this program simply run the MatrixReducer.exe file and it will prompt you to enter the size of the matrix (columns by rows, for instance a matrix that has 4 columns and 2 rows would be entered as "4 2"). After that it will ask for each row, separating the columns with a space. It will then reduce that matrix, and display some basic information on what it did, as well as the resulting matrix. There will also be a output.ltx file that will be the source for a LaTeX file that will describe each step that the program took, and show the matrix each step of the way.

Requirements
---------
The program is built using C# on the .NET framework, targeting .NET 4.0. You may need to download this if the program does not run on your computer, you can get it here if you are on windows [url:http://www.microsoft.com/en-us/download/details.aspx?id=24872} Linux and mac will have to use mono, which can be obtained from here: {url:http://www.go-mono.com/mono-downloads/download.html} Just select your OS, then download the runtime (you don't need the SDK or the mono develop unless you're planning on developing the application as well)

Planned Features
---------
One planned feature is to have the program also solve the corresponding linear system and explain the solution in the LaTeX file.

Support
----------
If you have problems with the program you can report the issues here, or email me at eldidip@gmail.com If you'd like to help with development you can comment here, or email me.