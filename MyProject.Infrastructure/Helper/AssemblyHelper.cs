using System.Reflection;

namespace MyProject.Infrastructure.Helper;

public class AssemblyHelper
{
    public static Assembly? GetAssembly(string assemblyName)
    {
        var rootAssembly = Assembly.GetEntryAssembly();

        if (rootAssembly == null)
            return null;

        var visited = new HashSet<string>();            
        var queue = new Queue<Assembly?>();

        queue.Enqueue(rootAssembly);

        while (queue.Any())
        {
            var assembly = queue.Dequeue();
            
            if (assembly != null && assembly.GetName().Name == assemblyName)
                return assembly;
            
            visited.Add(assembly.FullName);
            
            var references = assembly.GetReferencedAssemblies();
            foreach(var reference in references)
            {
                if (!visited.Contains(reference.FullName))            
                    queue.Enqueue(Assembly.Load(reference));
            }
        }
        
        return null;
    }
}