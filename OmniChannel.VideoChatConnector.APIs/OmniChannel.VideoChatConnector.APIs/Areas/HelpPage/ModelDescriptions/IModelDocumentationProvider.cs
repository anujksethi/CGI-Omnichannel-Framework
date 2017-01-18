using System;
using System.Reflection;

namespace OmniChannel.VideoChatConnector.APIs.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}