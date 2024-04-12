# CleanObjBinFolders

# Folder Deletion Console Application

This console application, built with .NET 8, is designed to delete folders based on a specified path. It utilizes dependency injection for flexible component management and Serilog for comprehensive logging capabilities.

## Features:
- **Folder Deletion:** Delete folders recursively based on the provided path.
- **Dependency Injection:** Utilize dependency injection for modular and testable code.
- **Logging with Serilog:** Implement Serilog for detailed logging to track application activities.

## Installation:

1. Clone the repository to your local machine.
2. Ensure you have .NET 8 SDK installed.
3. Build the solution using `dotnet build`.
4. Run the application using `dotnet run`.
   
## Usage:

1. Modify the `appsettings.json` file to configure logging options and other settings as needed.
2. Run the application and follow the prompts:
   - Provide the path of the folder you want to delete when prompted.
3. Monitor the logs generated by Serilog to track the execution progress and any errors encountered.

## Publishing:

To publish the application, follow these steps:

1. Navigate to the root of the solution.
2. Run `dotnet publish -c Release -o <output_directory>` to publish the application. Replace `<output_directory>` with the desired location for the published files.

## Dependencies:

- .NET 8 SDK
- Serilog
- Other dependencies specified in the `csproj` file

## Contributing:

Contributions are welcome! If you find any bugs or have suggestions for improvements, please open an issue or submit a pull request.

## License:

This project is licensed under the [MIT License](LICENSE).