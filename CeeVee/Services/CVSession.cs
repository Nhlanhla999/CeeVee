using CeeVee.Models;

namespace CeeVee.Services
{
    public static class CVSession
    {
        // The CV currently being edited
        public static CVDocument CurrentCV { get; private set; }

        // Currently selected template
        public static CVTemplateType SelectedTemplate
        {
            get
            {
                if (CurrentCV == null)
                    return CVTemplateType.Modern;

                return CurrentCV.SelectedTemplate;
            }
            set
            {
                if (CurrentCV != null)
                {
                    CurrentCV.SelectedTemplate = value;
                }
            }
        }


        // Initialize the session
        public static void Initialize()
        {
            if (CurrentCV == null)
            {
                NewCV();
            }
        }

        // Create a new CV
        public static void NewCV()
        {
            CurrentCV = new CVDocument();

            CurrentCV.SelectedTemplate = CVTemplateType.Modern;
        }
    }
}
