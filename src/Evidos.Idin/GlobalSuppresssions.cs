using System.Diagnostics.CodeAnalysis;

[assembly: Fody.ConfigureAwait(false)]
[assembly: SuppressMessage(
	"Reliability",
	"CA2007:Do not directly await a Task",
	Justification = "ConfigureAwait(false) is added at compilation by Fody",
	Scope = "namespaceanddescendants",
	Target = "Evidos.Idin")]
