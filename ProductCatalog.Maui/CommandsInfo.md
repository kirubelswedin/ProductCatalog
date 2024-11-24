
  * Commands:
  dotnet --info   
  dotnet --list-sdks
  xcodebuild -version
  dotnet restore
  dotnet workload list
  dotnet workload restore
  dotnet clean\nrm -rf bin/ obj/

  * Fixa iOS - Alla tillgängliga simulatorer:
  xcrun simctl list -> välj udid=****
  xcrun simctl boot 1CD99AEF-B90D-4BDD-92E7-4048AC21A7FD -> boot simulator
  xcrun simctl install 1CD99AEF-B90D-4BDD-92E7-4048AC21A7FD
  xcrun simctl install booted ProductCatalog.Maui/bin/Debug/net8.0-ios/iossimulator-arm64/ProductCatalog.Maui.app -> install app
  xcrun simctl launch 1CD99AEF-B90D-4BDD-92E7-4048AC21A7FD

  * Shell Script for iOS:
  dotnet build -t:Run -f net8.0-ios /p:_DeviceName=:v2:udid=1CD99AEF-B90D-4BDD-92E7-4048AC21A7FD
  dotnet build -t:Run -f net8.0-ios ProductCatalog.Maui/ProductCatalog.Maui.csproj /p:_DeviceName=:v2:udid=1CD99AEF-B90D-4BDD-92E7-4048AC21A7FD



