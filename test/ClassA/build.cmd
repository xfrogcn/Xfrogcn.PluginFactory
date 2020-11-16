dotnet build -o ../Abstractions.Test/bin/Debug/net5.0/plugin-a-n
rm ../Abstractions.Test/bin/Debug/net5.0/plugin-a-n/ClassA.deps.json -f
rm ../Abstractions.Test/bin/Debug/net5.0/plugin-a-n/ClassA.pdb -f
dotnet publish -o ../Abstractions.Test/bin/Debug/net5.0/plugin-a-i-e/ClassA
rm ../Abstractions.Test/bin/Debug/net5.0/plugin-a-i-e/ClassA/ClassA.deps.json -f
rm ../Abstractions.Test/bin/Debug/net5.0/plugin-a-i-e/ClassA/ClassA.pdb -f
dotnet publish --self-contained  -o ../Abstractions.Test/bin/Debug/net5.0/plugin-a-i/ClassA
