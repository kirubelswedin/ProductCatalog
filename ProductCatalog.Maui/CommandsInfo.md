
* Commands:
  dotnet --info   
  dotnet --list-sdks
  xcodebuild -version
  dotnet restore
  dotnet workload list
  dotnet workload restore
  dotnet clean\nrm -rf bin/ obj/

* iOS - All available simulators:
  xcrun simctl list -> ex. iPhone 16 (18.1)
  xcrun simctl boot 1CD99AEF-B90D-4BDD-92E7-4048AC21A7FD -> boot simulator
  xcrun simctl install booted ProductCatalog.Maui/bin/Debug/net8.0-ios/iossimulator-arm64/ProductCatalog.Maui.app -> install app

* Shell Script for iOS:
  Name: ex. ProductCatalogiOS
  Script text: dotnet build -t:Run -f net8.0-ios ProductCatalog.Maui/ProductCatalog.Maui.csproj /p:_DeviceName=:v2:udid=1CD99AEF-B90D-4BDD-92E7-4048AC21A7FD
  




