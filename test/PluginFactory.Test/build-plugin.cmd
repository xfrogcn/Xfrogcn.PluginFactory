dotnet publish ../../samples/TestPluginA -o ./bin/Debug/netcoreapp3.1/Plugins
rm -f ./bin/Debug/netcoreapp3.1/Plugins/Microsoft.*
rm -f ./bin/Debug/netcoreapp3.1/Plugins/PluginFactory.*
rm -f ./bin/Debug/netcoreapp3.1/Plugins/*.json