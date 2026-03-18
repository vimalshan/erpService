using System;
using System.Reflection;
using System.Linq;

var asm = typeof(Microsoft.OpenApi.IOpenApiElement).Assembly;
Console.WriteLine($""Assembly: {asm.GetName().Name} {asm.GetName().Version}"");

var types = asm.GetExportedTypes()
    .Where(t => t.Name.Contains(""Security"") || t.Name.Contains(""OpenApiInfo"") || t.Name.Contains(""OpenApiRef""))
    .Select(t => t.FullName)
    .OrderBy(n => n);

foreach (var t in types)
    Console.WriteLine(t);
