dotnet publish ../../samples/TestPluginA -o ./bin/Debug/net5.0/Plugins
rm -f ./bin/Debug/net5.0/Plugins/Microsoft.*
rm -f ./bin/Debug/net5.0/Plugins/Xfrogcn.PluginFactory.*
rm -f ./bin/Debug/net5.0/Plugins/*.json