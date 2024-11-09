// IMPORTANT: Do not change AssemblyInfo.cs. The file is generated automatically. 
// Apply any changes to AssemblyInfo.template instead.

using System;
using System.Runtime.CompilerServices;

[assembly: CLSCompliant(true)]

// Make internals visible to the unit test project.
#if !XBOX && !WINDOWS_PHONE
[assembly: InternalsVisibleTo("DigitalRune.Game.Tests")]
#endif