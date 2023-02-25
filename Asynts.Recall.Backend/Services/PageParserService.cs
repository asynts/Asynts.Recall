using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Services;

/// <summary>
/// Can be used to load <see cref="PageData" /> from files
/// </summary>
/// <remarks>
/// <para>
/// The input files are split into sections using an <code>---</code> indicator followed by the name of the section.
/// </para>
/// <para>
/// The following sections can be defined:
/// <list type="table">
/// <item>
/// <term>Metadata</term>
/// <description>
/// JSON document with the following properties: <c>id</c>, <c>title</c> and <c>tags</c>.
/// </description>
/// </item>
/// <item>
/// <term>Comment</term>
/// <description><em>(optional)</em> Can include additional information about the file itself, not indexed.</description>
/// </item>
/// <item>
/// <term>Summary</term>
/// <description>Summary of the contents is shown directly in search results.</description>
/// </item>
/// <item>
/// <term>Details</term>
/// <description><em>(optional)</em> The contents of the documents if they do not fit into the summary.
/// Only shown when an individual page is viewed.</description>
/// </item>
/// </list>
/// </para>
/// </remarks>
/// <example>
/// <code>
/// --- Metadata
/// {
///     "id": "37cf9645-ce5d-43cd-b5fc-3cd386860d32",
///     "title": "This is an example page.",
///     "tags": [ "example/foo/", "hello/" ]
/// }
/// --- Comment
/// This is not indexed by the system and can be used to comment on the file itself.
/// --- Summary
/// This is the summary that is shown in the search results.
/// --- Details
/// This can be very long and is only shown when an individual page is viewed.
/// </code>
/// </example>
public class PageParserService
{
    public PageParserService()
    {

    }

    public PageData Parse(string input)
    {
        // FIXME
        throw new NotImplementedException();
    }
}
