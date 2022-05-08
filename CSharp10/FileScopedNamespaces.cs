
/*
    File Scoped Namespaces
    ---------------------------------------------------------------------------------------------------------------- 
    
    File Scoped Namespaces allow the definition of a namespace for a file in a more concise format, 
    removing 1 tab level from all code inside the file.
    
    They are declared as:
    
        namespace MyNamespace;
    
    This line places any declared types into that namespace
    
    A hierarchy of namespaces can also be defined, as with the standard form.
    
        namespace MyCompany.MyProduct.MyNamespace;
    
    Cannot be mixed
    ---------------------------------------------------------------------------------------------------------------- 
    
    A file cannot contain both file-scoped namespaces and normal namespace declarations
    so the following would be a compile-time error:
    
        namespace CSharp;
        
        namespace CSharp // compile-time error
        {
        }
    
    Cannot be nested
    ---------------------------------------------------------------------------------------------------------------- 
    
    File scoped namespaces cannot be nested, regardless of whether the nested namespace is file-scoped or a standard
    namespace, so the following will produce compile-time errors:
    
        namespace MyProduct;
        namespace MyNamespace; // Compile-time error
        
        namespace MyOtherNamespace // Compile-time error
        {
        }
    
    Must precede all other members
    ---------------------------------------------------------------------------------------------------------------- 
    
    The file-scoped namespace declaration must precede all other member declarations, the following will produce
    a compile time error:
    
        class Program
        {
        }
        
        namespace MyNamespace; // Compile-time error
    
    'using' declarations are allowed to be placed before the namespace declaration, the following will compile:
    
        using System;
    
        namespace MyNamespace;
        
    The above is semantically equivalent to the following, which is also allowed:
    
        namespace MyNamespace;
        
        using System;
 */

namespace CSharp;      
