using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using CeeVee.Models;
using CeeVee.Services;
using CeeVee.Views.Templates;


namespace CeeVee
{
    public partial class MainWindow : Window
    {
        private int currentStep = 1;
        private int currentExperienceIndex = 0;
        private int currentEducationIndex = 0;
        private int currentProjectIndex = 0;
        private int currentCertificateIndex = 0;
        private int currentAchievementIndex = 0;
        private int currentReferenceIndex = 0;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public MainWindow()
        {
            InitializeComponent();

            CVSession.Initialize();

            DataContext = CVSession.CurrentCV;

            InitializeExperience();

            InitializeEducation();

            InitializeOptionalCollections();

            SetInitialTemplate();

            ShowStep(1);

        }

        private void ShowStep(int step)
        {
            currentStep = step;

            PersonalStep.Visibility = Visibility.Collapsed;

            ProfileStep.Visibility = Visibility.Collapsed;

            ExperienceStep.Visibility = Visibility.Collapsed;

            EducationStep.Visibility = Visibility.Collapsed;

            SkillsStep.Visibility = Visibility.Collapsed;

            OptionalStep.Visibility = Visibility.Collapsed;

            FinishStep.Visibility = Visibility.Collapsed;


            switch (step)
            {
                case 1:

                    PersonalStep.Visibility = Visibility.Visible;

                    StepDescription.Text = "Let's start with your personal information.";

                    break;


                case 2:

                    ProfileStep.Visibility = Visibility.Visible;

                    StepDescription.Text = "Tell employers who you are.";

                    break;


                case 3:

                    ExperienceStep.Visibility = Visibility.Visible;

                    StepDescription.Text = "Add your professional work history.";

                    LoadCurrentExperience();

                    break;


                case 4:

                    EducationStep.Visibility = Visibility.Visible;

                    StepDescription.Text = "Add your academic qualifications.";

                    LoadCurrentEducation();

                    break;


                case 5:

                    SkillsStep.Visibility = Visibility.Visible;

                    StepDescription.Text = "Highlight your skills and languages.";

                    break;


                case 6:

                    OptionalStep.Visibility = Visibility.Visible;

                    StepDescription.Text = "Add any extra sections you want. Everything here is optional.";

                    break;


                case 7:

                    FinishStep.Visibility = Visibility.Visible;

                    StepDescription.Text = "Review your CV and save it.";

                    if (SelectedTemplateText != null)
                    {
                        SelectedTemplateText.Text = CVSession.CurrentCV.SelectedTemplate.ToString();
                    }

                    break;
            }


            UpdateStepIndicators();


            BackButton.IsEnabled = step > 1;


            if (step == 7)
            {
                NextButton.Content = "Save CV";
            }
            else
            {
                NextButton.Content = "Next  →";
            }
        }


        // =========================================================
        // STEP INDICATORS
        // =========================================================

        private void UpdateStepIndicators()
        {
            ResetStepIndicator(Step1Circle);
            ResetStepIndicator(Step2Circle);
            ResetStepIndicator(Step3Circle);
            ResetStepIndicator(Step4Circle);
            ResetStepIndicator(Step5Circle);
            ResetStepIndicator(Step6Circle);
            ResetStepIndicator(Step7Circle);


            Border activeCircle = null;


            switch (currentStep)
            {
                case 1:
                    activeCircle = Step1Circle;
                    break;

                case 2:
                    activeCircle = Step2Circle;
                    break;

                case 3:
                    activeCircle = Step3Circle;
                    break;

                case 4:
                    activeCircle = Step4Circle;
                    break;

                case 5:
                    activeCircle = Step5Circle;
                    break;

                case 6:
                    activeCircle = Step6Circle;
                    break;

                case 7:
                    activeCircle = Step7Circle;
                    break;
            }


            if (activeCircle != null)
            {
                activeCircle.Background = new SolidColorBrush(Color.FromRgb(37,35,33));
            }
        }


        private void ResetStepIndicator(Border circle)
        {
            if (circle == null)
                return;

            circle.Background = new SolidColorBrush(Color.FromRgb(221,215,206));
        }


        // =========================================================
        // CURRENT STEP
        // =========================================================

        private void SaveCurrentStep()
        {
        }

        // =========================================================
        // EXPERIENCE
        // =========================================================

        private void InitializeExperience()
        {
            if (CVSession.CurrentCV == null)
                return;

            if (CVSession.CurrentCV.Experiences == null)
            {
                CVSession.CurrentCV.Experiences = new ObservableCollection<Experience>();
            }

            if (CVSession.CurrentCV.Experiences.Count == 0)
            {
                CVSession.CurrentCV.Experiences.Add(new Experience());
            }

            currentExperienceIndex = 0;

            LoadCurrentExperience();
        }



        private void LoadCurrentExperience()
        {
            if (CVSession.CurrentCV.Experiences.Count == 0)
            {
                ExperienceEditorPanel.Visibility = Visibility.Collapsed;

                ExperienceNavigationPanel.Visibility = Visibility.Collapsed;

                AddFirstExperienceButton.Visibility = Visibility.Visible;

                return;
            }

            if (currentExperienceIndex < 0)
            {
                currentExperienceIndex = 0;
            }

            if (currentExperienceIndex >= CVSession.CurrentCV.Experiences.Count)
            {
                currentExperienceIndex = CVSession.CurrentCV.Experiences.Count - 1;
            }

            ExperienceEditorPanel.Visibility = Visibility.Visible;

            ExperienceNavigationPanel.Visibility = Visibility.Visible;

            AddFirstExperienceButton.Visibility = Visibility.Collapsed;

            Experience currentExperience = CVSession.CurrentCV.Experiences[currentExperienceIndex];

            ExperienceEditorPanel.DataContext = currentExperience;

            ConfigureExperienceDatePickers();

            ExperienceCounter.Text = "Experience " + (currentExperienceIndex + 1) + " of " + CVSession.CurrentCV.Experiences.Count;

            PreviousExperienceButton.IsEnabled = currentExperienceIndex > 0;

            NextExperienceButton.IsEnabled = currentExperienceIndex < CVSession.CurrentCV.Experiences.Count - 1;
        }


        private void ConfigureExperienceDatePickers()
        {
            Experience experience = ExperienceEditorPanel.DataContext as Experience;

            if (experience == null)
                return;

            DateTime currentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            ExperienceStartDate.SelectedMonth = experience.StartDate;

            ExperienceStartDate.MaximumMonth = currentMonth;

            ExperienceEndDate.SelectedMonth = experience.EndDate;

            ExperienceEndDate.MaximumMonth = currentMonth;

            // End cannot be earlier than Start.
            if (experience.StartDate.HasValue)
            {
                ExperienceEndDate.MinimumMonth = experience.StartDate;
            }
            else
            {
                ExperienceEndDate.MinimumMonth =
                    null;
            }


            // ============================================================
            // EVENT HANDLERS
            // ============================================================

            ExperienceStartDate.SelectionChanged -= ExperienceStartDate_SelectionChanged;

            ExperienceStartDate.SelectionChanged += ExperienceStartDate_SelectionChanged;

            ExperienceEndDate.SelectionChanged -= ExperienceEndDate_SelectionChanged;

            ExperienceEndDate.SelectionChanged += ExperienceEndDate_SelectionChanged;

            UpdateExperienceDatePickers();
        }

        private void ExperienceStartDate_SelectionChanged(object sender, EventArgs e)
        {
            Experience experience = ExperienceEditorPanel.DataContext as Experience;

            if (experience == null)
                return;

            experience.StartDate = ExperienceStartDate.SelectedMonth;


            // ============================================================
            // END DATE CANNOT BE BEFORE START DATE
            // ============================================================

            ExperienceEndDate.MinimumMonth = experience.StartDate;


            if (experience.StartDate.HasValue && experience.EndDate.HasValue && experience.EndDate.Value < experience.StartDate.Value)
            {
                experience.EndDate = experience.StartDate;

                ExperienceEndDate.SelectedMonth = experience.StartDate;
            }

            UpdateExperienceDateValidation();
        }


        private void ExperienceEndDate_SelectionChanged(object sender, EventArgs e)
        {
            Experience experience = ExperienceEditorPanel.DataContext as Experience;

            if (experience == null)
                return;

            DateTime? selectedEnd = ExperienceEndDate.SelectedMonth;


            // ============================================================
            // NEVER ALLOW END BEFORE START
            // ============================================================

            if (experience.StartDate.HasValue && selectedEnd.HasValue && selectedEnd.Value < experience.StartDate.Value)
            {
                ExperienceEndDate.SelectedMonth = experience.StartDate;

                experience.EndDate = experience.StartDate;

                return;
            }


            experience.EndDate = selectedEnd;

            UpdateExperienceDateValidation();
        }


        private void ExperiencePresent_Checked(object sender, RoutedEventArgs e)
        {
            Experience experience = ExperienceEditorPanel.DataContext as Experience;

            if (experience == null)
                return;

            experience.IsPresent = true;

            ExperienceEndDate.IsEnabled = false;

            ExperienceEndDate.SelectedMonth = null;

            experience.EndDate = null;

            UpdateExperienceDateValidation();
        }


        private void ExperiencePresent_Unchecked(object sender, RoutedEventArgs e)
        {
            Experience experience = ExperienceEditorPanel.DataContext as Experience;

            if (experience == null)
                return;

            experience.IsPresent = false;

            ExperienceEndDate.IsEnabled = true;

            ExperienceEndDate.MaximumMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            if (experience.StartDate.HasValue)
            {
                ExperienceEndDate.MinimumMonth = experience.StartDate;
            }

            UpdateExperienceDateValidation();
        }

        private void UpdateExperienceDatePickers()
        {
            Experience experience = ExperienceEditorPanel.DataContext as Experience;

            if (experience == null)
                return;


            DateTime currentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            ExperienceStartDate.MaximumMonth = currentMonth;

            ExperienceStartDate.SelectedMonth = experience.StartDate;

            ExperienceEndDate.MaximumMonth = currentMonth;

            ExperienceEndDate.MinimumMonth = experience.StartDate;

            ExperienceEndDate.SelectedMonth = experience.EndDate;

            ExperienceEndDate.IsEnabled = !experience.IsPresent;

            if (experience.IsPresent)
            {
                ExperienceEndDate.SelectedMonth = null;
            }


            UpdateExperienceDateValidation();
        }


        private void UpdateExperienceDateValidation()
        {
            Experience experience = ExperienceEditorPanel.DataContext as Experience;

            if (experience == null)
                return;


            ExperienceDateValidationText.Visibility = Visibility.Collapsed;

            ExperienceDateValidationText.Text = "";


            if (!experience.StartDate.HasValue)
                return;


            if (experience.IsPresent)
                return;


            if (!experience.EndDate.HasValue)
                return;


            if (experience.EndDate.Value < experience.StartDate.Value)
            {
                ExperienceDateValidationText.Text = "End date cannot be earlier than the start date.";

                ExperienceDateValidationText.Visibility = Visibility.Visible;
            }
        }


        private void PreviousExperience_Click(object sender, RoutedEventArgs e)
        {
            if (currentExperienceIndex <= 0)
                return;

            currentExperienceIndex--;

            LoadCurrentExperience();
        }


        private void NextExperience_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV.Experiences == null)
                return;


            if (currentExperienceIndex >= CVSession.CurrentCV.Experiences.Count - 1)
            {
                return;
            }


            currentExperienceIndex++;

            LoadCurrentExperience();
        }


        private void AddExperience_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV.Experiences == null)
            {
                CVSession.CurrentCV.Experiences = new ObservableCollection<Experience>();
            }


            Experience newExperience = new Experience();


            CVSession.CurrentCV.Experiences.Add(newExperience);


            currentExperienceIndex =CVSession.CurrentCV.Experiences.Count - 1;


            LoadCurrentExperience();


            if (ExperienceJobTitle != null)
            {
                ExperienceJobTitle.Focus();
            }
        }


        private void RemoveExperience_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV.Experiences == null)
                return;

            // Keep at least one experience editor available.
            if (CVSession.CurrentCV.Experiences.Count == 0)
            {
                return;
            }

            CVSession.CurrentCV.Experiences.RemoveAt(currentExperienceIndex);

            if (CVSession.CurrentCV.Experiences.Count == 0)
            {
                currentEducationIndex = 0;


                ExperienceEditorPanel.Visibility = Visibility.Collapsed;

                ExperienceNavigationPanel.Visibility = Visibility.Collapsed;

                AddFirstExperienceButton.Visibility = Visibility.Visible;

                return;
            }

            // Move to the previous item if the last item was removed.
            if (currentExperienceIndex >= CVSession.CurrentCV.Experiences.Count)
            {
                currentExperienceIndex = CVSession.CurrentCV.Experiences.Count - 1;
            }

            LoadCurrentExperience();
        }




        // =========================================================
        // EDUCATION
        // =========================================================

        private void InitializeEducation()
        {
            if (CVSession.CurrentCV.Educations == null)
            {
                CVSession.CurrentCV.Educations = new ObservableCollection<Education>();
            }


            if (CVSession.CurrentCV.Educations.Count == 0)
            {
                CVSession.CurrentCV.Educations.Add(new Education());
            }


            currentEducationIndex = 0;

            SetCurrentEducation();
        }


        private void SetCurrentEducation()
        {
            if (CVSession.CurrentCV == null)
                return;

            if (CVSession.CurrentCV.Educations == null)
                return;

            if (CVSession.CurrentCV.Educations.Count == 0)
                return;


            if (currentEducationIndex < 0)
            {
                currentEducationIndex = 0;
            }


            if (currentEducationIndex >= CVSession.CurrentCV.Educations.Count)
            {
                currentEducationIndex = CVSession.CurrentCV.Educations.Count - 1;
            }


            if (EducationEditorPanel != null)
            {
                EducationEditorPanel.DataContext = CVSession.CurrentCV.Educations[currentEducationIndex];
            }


            if (EducationCounter != null)
            {
                EducationCounter.Text = "Education " + (currentEducationIndex + 1) + " of " + CVSession.CurrentCV.Educations.Count;
            }


            UpdateEducationButtons();
        }


        private void LoadCurrentEducation()
        {
            if (CVSession.CurrentCV.Educations.Count == 0)
            {
                EducationEditorPanel.Visibility = Visibility.Collapsed;

                EducationNavigationPanel.Visibility = Visibility.Collapsed;

                AddFirstEducationButton.Visibility = Visibility.Visible;

                return;
            }


            if (currentEducationIndex < 0)
            {
                currentEducationIndex = 0;
            }


            if (currentEducationIndex >= CVSession.CurrentCV.Educations.Count)
            {
                currentEducationIndex = CVSession.CurrentCV.Educations.Count - 1;
            }

            EducationEditorPanel.Visibility = Visibility.Visible;

            EducationNavigationPanel.Visibility = Visibility.Visible;

            AddFirstEducationButton.Visibility = Visibility.Collapsed;


            Education currentEducation = CVSession.CurrentCV.Educations[currentEducationIndex];


            EducationEditorPanel.DataContext = currentEducation;

            EducationPresent.IsChecked = currentEducation.IsPresent;


            ConfigureEducationDatePicker();


            EducationCounter.Text ="Education " + (currentEducationIndex + 1) + " of " + CVSession.CurrentCV.Educations.Count;

            PreviousEducationButton.IsEnabled = currentEducationIndex > 0;

            NextEducationButton.IsEnabled = currentEducationIndex < CVSession.CurrentCV.Educations.Count - 1;
        }


        private void ConfigureEducationDatePicker()
        {
            Education education = EducationEditorPanel.DataContext as Education;


            if (education == null)
                return;


            DateTime currentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);


            EducationYear.MaximumMonth = currentMonth;

            EducationYear.MinimumMonth = null;


            EducationYear.SelectedMonth = education.Year;

            EducationYear.SelectionChanged -= EducationYear_SelectionChanged;

            EducationYear.SelectionChanged += EducationYear_SelectionChanged;


            EducationYear.IsEnabled = !education.IsPresent;


            UpdateEducationDateValidation();
        }

        private void EducationYear_SelectionChanged(object sender, EventArgs e)
        {
            Education education = EducationEditorPanel.DataContext as Education;


            if (education == null)
                return;


            if (EducationYear.SelectedMonth.HasValue)
            {
                education.Year = EducationYear.SelectedMonth;
            }
            else
            {
                education.Year = null;
            }


            UpdateEducationDateValidation();
        }


        private void EducationPresent_Checked(object sender, RoutedEventArgs e)
        {
            Education education = EducationEditorPanel.DataContext as Education;


            if (education == null)
                return;


            education.IsPresent = true;


            EducationYear.IsEnabled = false;


            EducationYear.SelectedMonth = null;


            education.Year = null;


            EducationDateValidationText.Visibility = Visibility.Collapsed;
        }

        private void EducationPresent_Unchecked(object sender, RoutedEventArgs e)
        {
            Education education = EducationEditorPanel.DataContext as Education;


            if (education == null)
                return;


            education.IsPresent = false;


            EducationYear.IsEnabled = true;


            EducationYear.MaximumMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            UpdateEducationDateValidation();
        }


        private void UpdateEducationDateValidation()
        {
            Education education = EducationEditorPanel.DataContext as Education;


            if (education == null)
                return;


            EducationDateValidationText.Visibility = Visibility.Collapsed;

            EducationDateValidationText.Text = "";


            if (education.IsPresent)
                return;


            if (!education.Year.HasValue)
                return;


            DateTime currentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);


            if (education.Year.Value > currentMonth)
            {
                EducationDateValidationText.Text = "Education date cannot be in the future.";

                EducationDateValidationText.Visibility = Visibility.Visible;
            }
        }


        private void PreviousEducation_Click(object sender, RoutedEventArgs e)
        {
            if (currentEducationIndex <= 0)
                return;

            currentEducationIndex--;

            LoadCurrentEducation();
        }


        private void NextEducation_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV.Educations == null)
                return;


            if (currentEducationIndex >= CVSession.CurrentCV.Educations.Count - 1)
            {
                return;
            }


            currentEducationIndex++;

            LoadCurrentEducation();
        }


        private void AddEducation_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV.Educations == null)
            {
                CVSession.CurrentCV.Educations = new ObservableCollection<Education>();
            }


            Education newEducation = new Education();


            CVSession.CurrentCV.Educations.Add(newEducation);


            currentEducationIndex = CVSession.CurrentCV.Educations.Count - 1;


            LoadCurrentEducation();


            if (EducationDegree != null)
            {
                EducationDegree.Focus();
            }
        }


        private void RemoveEducation_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV.Educations == null)
                return;

            // Keep at least one education editor available.
            if (CVSession.CurrentCV.Educations.Count == 0)
            {

                return;
            }



            CVSession.CurrentCV.Educations.RemoveAt(currentEducationIndex);

            if (CVSession.CurrentCV.Educations.Count == 0)
            {
                currentEducationIndex = 0;


                EducationEditorPanel.Visibility = Visibility.Collapsed;

                EducationNavigationPanel.Visibility = Visibility.Collapsed;

                AddFirstEducationButton.Visibility = Visibility.Visible;

                return;
            }


            // Move to the previous item if the last item was removed.
            if (currentEducationIndex >= CVSession.CurrentCV.Educations.Count)
            {
                currentEducationIndex = CVSession.CurrentCV.Educations.Count - 1;
            }

            LoadCurrentEducation();
        }


        private void UpdateEducationButtons()
        {
            if (PreviousEducationButton == null || NextEducationButton == null)
            {
                return;
            }


            if (CVSession.CurrentCV.Educations == null || CVSession.CurrentCV.Educations.Count == 0)
            {
                PreviousEducationButton.IsEnabled = false;

                NextEducationButton.IsEnabled = false;

                return;
            }


            PreviousEducationButton.IsEnabled = currentEducationIndex > 0;


            NextEducationButton.IsEnabled = currentEducationIndex < CVSession.CurrentCV.Educations.Count - 1;
        }


        // =========================================================
        // SKILLS
        // =========================================================

        private void AddSkill_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV == null)
            {
                MessageBox.Show("No CV is currently loaded.");
                return;
            }

            if (CVSession.CurrentCV.Skills == null)
            {
                CVSession.CurrentCV.Skills =
                    new ObservableCollection<Skill>();
            }

            CVSession.CurrentCV.Skills.Add(
                new Skill
                {
                    Name = ""
                });

            // Refresh the UI binding if necessary
            if (DataContext == CVSession.CurrentCV)
            {
                return;
            }

            // Ensure the UI is bound to the current CV
            DataContext = CVSession.CurrentCV;
        }


        private void RemoveSkill_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
                return;

            Skill skill = button.DataContext as Skill;

            if (skill == null)
                return;

            CVSession.CurrentCV.Skills.Remove(skill);
        }


        private void AddLanguage_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV == null)
            {
                MessageBox.Show("No CV is currently loaded.");
                return;
            }

            if (CVSession.CurrentCV.Languages == null)
            {
                CVSession.CurrentCV.Languages =
                    new ObservableCollection<Language>();
            }

            CVSession.CurrentCV.Languages.Add(
                new Language
                {
                    Name = ""
                });

            // Refresh the UI binding if necessary
            if (DataContext == CVSession.CurrentCV)
            {
                return;
            }

            // Ensure the UI is bound to the current CV
            DataContext = CVSession.CurrentCV;
        }


        private void RemoveLanguage_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
                return;

            Language language = button.DataContext as Language;

            if (language == null)
                return;

            CVSession.CurrentCV.Languages.Remove(language);
        }

        // =========================================================
        // PROJECTS
        // =========================================================
        private void LoadCurrentProject()
        {
            if (CVSession.CurrentCV.Projects == null)
            {
                CVSession.CurrentCV.Projects = new System.Collections.ObjectModel.ObservableCollection<Project>();
            }

            if (CVSession.CurrentCV.Projects.Count == 0)
            {
                ProjectEditorPanel.Visibility = Visibility.Collapsed;

                ProjectNavigationPanel.Visibility = Visibility.Collapsed;

                AddFirstProjectButton.Visibility = Visibility.Visible;

                return;
            }

            if (currentProjectIndex < 0)
            {
                currentProjectIndex = 0;
            }

            if (currentProjectIndex >= CVSession.CurrentCV.Projects.Count)
            {
                currentProjectIndex = CVSession.CurrentCV.Projects.Count - 1;
            }

            ProjectEditorPanel.Visibility = Visibility.Visible;

            ProjectNavigationPanel.Visibility = Visibility.Visible;

            AddFirstProjectButton.Visibility = Visibility.Collapsed;

            ProjectEditorPanel.DataContext = CVSession.CurrentCV.Projects[currentProjectIndex];

            ProjectCounter.Text = "Project " + (currentProjectIndex + 1) + " of " + CVSession.CurrentCV.Projects.Count;

            UpdateProjectButtons();
        }

        private void UpdateProjectButtons()
        {
            if (CVSession.CurrentCV.Projects == null)
                return;

            int count = CVSession.CurrentCV.Projects.Count;

            PreviousProjectButton.IsEnabled = currentProjectIndex > 0;

            NextProjectButton.IsEnabled = currentProjectIndex < count - 1;
        }


        private void PreviousProject_Click(object sender, RoutedEventArgs e)
        {
            if (currentProjectIndex <= 0)
                return;

            currentProjectIndex--;

            LoadCurrentProject();
        }

        private void NextProject_Click(object sender, RoutedEventArgs e)
        {
            if (currentProjectIndex >= CVSession.CurrentCV.Projects.Count - 1)
            {
                return;
            }

            currentProjectIndex++;

            LoadCurrentProject();
        }

        private void AddProject_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV.Projects == null)
            {
                CVSession.CurrentCV.Projects = new System.Collections.ObjectModel.ObservableCollection<Project>();
            }

            // Create a new project
            Project newProject = new Project();

            CVSession.CurrentCV.Projects.Add(newProject);

            currentProjectIndex = CVSession.CurrentCV.Projects.Count - 1;

            ProjectEditorPanel.Visibility = Visibility.Visible;

            ProjectNavigationPanel.Visibility = Visibility.Visible;

            AddFirstProjectButton.Visibility = Visibility.Collapsed;

            // Load the new project
            LoadCurrentProject();
        }


        private void RemoveProject_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV.Projects == null)
                return;

            if (CVSession.CurrentCV.Projects.Count == 0)
                return;

            CVSession.CurrentCV.Projects.RemoveAt(currentProjectIndex);

            if (CVSession.CurrentCV.Projects.Count == 0)
            {
                currentProjectIndex = 0;

                ProjectEditorPanel.Visibility = Visibility.Collapsed;

                ProjectNavigationPanel.Visibility = Visibility.Collapsed;

                AddFirstProjectButton.Visibility = Visibility.Visible;

                return;
            }

            if (currentProjectIndex >= CVSession.CurrentCV.Projects.Count)
            {
                currentProjectIndex = CVSession.CurrentCV.Projects.Count - 1;
            }

            LoadCurrentProject();
        }


        // =========================================================
        // CERTIFICATES
        // =========================================================
        private void LoadCurrentCertificate()
        {
            if (CVSession.CurrentCV.Certificates == null)
            {
                CVSession.CurrentCV.Certificates = new System.Collections.ObjectModel.ObservableCollection<Certificate>();
            }


            // No certificates
            if (CVSession.CurrentCV.Certificates.Count == 0)
            {
                CertificateEditorPanel.Visibility = Visibility.Collapsed;

                CertificateNavigationPanel.Visibility = Visibility.Collapsed;

                AddFirstCertificateButton.Visibility = Visibility.Visible;

                return;
            }

            if (currentCertificateIndex < 0)
            {
                currentCertificateIndex = 0;
            }


            if (currentCertificateIndex >= CVSession.CurrentCV.Certificates.Count)
            {
                currentCertificateIndex = CVSession.CurrentCV.Certificates.Count - 1;
            }

            CertificateEditorPanel.Visibility = Visibility.Visible;

            CertificateNavigationPanel.Visibility = Visibility.Visible;

            AddFirstCertificateButton.Visibility = Visibility.Collapsed;

            CertificateEditorPanel.DataContext = CVSession.CurrentCV.Certificates[currentCertificateIndex];

            CertificateCounter.Text = "Certificate " + (currentCertificateIndex + 1) + " of " + CVSession.CurrentCV.Certificates.Count;


            UpdateCertificateButtons();
        }

        private void UpdateCertificateButtons()
        {
            if (CVSession.CurrentCV.Certificates == null)
                return;

            int count = CVSession.CurrentCV.Certificates.Count;


            PreviousCertificateButton.IsEnabled = currentCertificateIndex > 0;


            NextCertificateButton.IsEnabled = currentCertificateIndex < count - 1;
        }

        private void AddCertificate_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV.Certificates == null)
            {
                CVSession.CurrentCV.Certificates = new System.Collections.ObjectModel.ObservableCollection<Certificate>();
            }


            Certificate newCertificate = new Certificate();


            CVSession.CurrentCV.Certificates.Add(newCertificate);

            currentCertificateIndex = CVSession.CurrentCV.Certificates.Count - 1;


            LoadCurrentCertificate();
        }

        private void PreviousCertificate_Click(object sender, RoutedEventArgs e)
        {
            if (currentCertificateIndex <= 0)
                return;


            currentCertificateIndex--;


            LoadCurrentCertificate();
        }


        private void NextCertificate_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV.Certificates == null)
                return;


            if (currentCertificateIndex >= CVSession.CurrentCV.Certificates.Count - 1)
            {
                return;
            }


            currentCertificateIndex++;


            LoadCurrentCertificate();
        }


        private void RemoveCertificate_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV.Certificates == null)
                return;


            if (CVSession.CurrentCV.Certificates.Count == 0)
                return;


            CVSession.CurrentCV.Certificates.RemoveAt(currentCertificateIndex);


            // No certificates remain
            if (CVSession.CurrentCV.Certificates.Count == 0)
            {
                currentCertificateIndex = 0;


                CertificateEditorPanel.Visibility = Visibility.Collapsed;

                CertificateNavigationPanel.Visibility = Visibility.Collapsed;

                AddFirstCertificateButton.Visibility = Visibility.Visible;

                return;
            }


            // If the last certificate was removed,
            // move to the new last certificate.
            if (currentCertificateIndex >= CVSession.CurrentCV.Certificates.Count)
            {
                currentCertificateIndex = CVSession.CurrentCV.Certificates.Count - 1;
            }


            LoadCurrentCertificate();
        }


        // =========================================================
        // ACHIEVEMENTS
        // =========================================================
        private void LoadCurrentAchievement()
        {
            if (CVSession.CurrentCV.Achievements == null)
            {
                CVSession.CurrentCV.Achievements = new System.Collections.ObjectModel.ObservableCollection<Achievement>();
            }


            // No achievements
            if (CVSession.CurrentCV.Achievements.Count == 0)
            {
                AchievementEditorPanel.Visibility = Visibility.Collapsed;

                AchievementNavigationPanel.Visibility = Visibility.Collapsed;

                AddFirstAchievementButton.Visibility = Visibility.Visible;

                return;
            }


            if (currentAchievementIndex < 0)
            {
                currentAchievementIndex = 0;
            }


            if (currentAchievementIndex >= CVSession.CurrentCV.Achievements.Count)
            {
                currentAchievementIndex = CVSession.CurrentCV.Achievements.Count - 1;
            }

            AchievementEditorPanel.Visibility = Visibility.Visible;

            AchievementNavigationPanel.Visibility = Visibility.Visible;

            AddFirstAchievementButton.Visibility = Visibility.Collapsed;


            // Bind current achievement
            AchievementEditorPanel.DataContext = CVSession.CurrentCV.Achievements[currentAchievementIndex];


            // Counter
            AchievementCounter.Text = "Achievement " + (currentAchievementIndex + 1) + " of " + CVSession.CurrentCV.Achievements.Count;

            UpdateAchievementButtons();
        }

        private void UpdateAchievementButtons()
        {
            if (CVSession.CurrentCV.Achievements == null)
                return;


            int count = CVSession.CurrentCV.Achievements.Count;


            PreviousAchievementButton.IsEnabled = currentAchievementIndex > 0;


            NextAchievementButton.IsEnabled = currentAchievementIndex < count - 1;
        }

        private void AddAchievement_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV.Achievements == null)
            {
                CVSession.CurrentCV.Achievements = new System.Collections.ObjectModel.ObservableCollection<Achievement>();
            }


            Achievement newAchievement = new Achievement();


            CVSession.CurrentCV.Achievements.Add(newAchievement);


            currentAchievementIndex = CVSession.CurrentCV.Achievements.Count - 1;


            LoadCurrentAchievement();
        }

        private void PreviousAchievement_Click(object sender, RoutedEventArgs e)
        {
            if (currentAchievementIndex <= 0)
                return;


            currentAchievementIndex--;


            LoadCurrentAchievement();
        }

        private void NextAchievement_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV.Achievements == null)
                return;


            if (currentAchievementIndex >= CVSession.CurrentCV.Achievements.Count - 1)
            {
                return;
            }


            currentAchievementIndex++;


            LoadCurrentAchievement();
        }


        private void RemoveAchievement_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV.Achievements == null)
                return;


            if (CVSession.CurrentCV.Achievements.Count == 0)
                return;


            CVSession.CurrentCV.Achievements.RemoveAt(currentAchievementIndex);


            // No achievements remain
            if (CVSession.CurrentCV.Achievements.Count == 0)
            {
                currentAchievementIndex = 0;


                AchievementEditorPanel.Visibility = Visibility.Collapsed;

                AchievementNavigationPanel.Visibility = Visibility.Collapsed;

                AddFirstAchievementButton.Visibility = Visibility.Visible;

                return;
            }

            if (currentAchievementIndex >= CVSession.CurrentCV.Achievements.Count)
            {
                currentAchievementIndex = CVSession.CurrentCV.Achievements.Count - 1;
            }
            LoadCurrentAchievement();
        }


        // =========================================================
        // REFERENCES
        // =========================================================


        private void LoadCurrentReference()
        {
            if (CVSession.CurrentCV.References == null)
            {
                CVSession.CurrentCV.References = new System.Collections.ObjectModel.ObservableCollection<Reference>();
            }



            // =============================================
            // LIST REFERENCES
            // =============================================



            // No references
            if (CVSession.CurrentCV.References.Count == 0)
            {
                ReferenceEditorPanel.Visibility = Visibility.Collapsed;

                ReferenceNavigationPanel.Visibility = Visibility.Collapsed;

                AddFirstReferenceButton.Visibility = Visibility.Visible;

                return;
            }


            // Keep index valid
            if (currentReferenceIndex < 0)
            {
                currentReferenceIndex = 0;
            }

            if (currentReferenceIndex >=
                CVSession.CurrentCV.References.Count)
            {
                currentReferenceIndex = CVSession.CurrentCV.References.Count - 1;
            }

            ReferenceEditorPanel.Visibility = Visibility.Visible;

            ReferenceNavigationPanel.Visibility = Visibility.Visible;

            AddFirstReferenceButton.Visibility = Visibility.Collapsed;


            // Bind current reference
            ReferenceEditorPanel.DataContext = CVSession.CurrentCV.References[currentReferenceIndex];

            // Counter
            ReferenceCounter.Text ="Reference " + (currentReferenceIndex + 1) +" of " + CVSession.CurrentCV.References.Count;


            UpdateReferenceButtons();
        }


        private void UpdateReferenceButtons()
        {
            if (CVSession.CurrentCV.References == null)
                return;


            int count = CVSession.CurrentCV.References.Count;

            PreviousReferenceButton.IsEnabled = currentReferenceIndex > 0;

            NextReferenceButton.IsEnabled = currentReferenceIndex < count - 1;
        }

        private void AddReference_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV.References == null)
            {
                CVSession.CurrentCV.References = new System.Collections.ObjectModel.ObservableCollection<Reference>();
            }


            Reference newReference = new Reference();


            CVSession.CurrentCV.References.Add(newReference);

            currentReferenceIndex = CVSession.CurrentCV.References.Count - 1;


            LoadCurrentReference();
        }


        private void PreviousReference_Click(object sender, RoutedEventArgs e)
        {
            if (currentReferenceIndex <= 0)
                return;


            currentReferenceIndex--;


            LoadCurrentReference();
        }

        private void NextReference_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV.References == null)
                return;


            if (currentReferenceIndex >= CVSession.CurrentCV.References.Count - 1)
            {
                return;
            }


            currentReferenceIndex++;


            LoadCurrentReference();
        }

        private void RemoveReference_Click(object sender, RoutedEventArgs e)
        {
            if (CVSession.CurrentCV.References == null)
                return;


            if (CVSession.CurrentCV.References.Count == 0)
                return;


            CVSession.CurrentCV.References.RemoveAt(currentReferenceIndex);


            if (CVSession.CurrentCV.References.Count == 0)
            {
                currentReferenceIndex = 0;

                ReferenceEditorPanel.Visibility = Visibility.Collapsed;

                ReferenceNavigationPanel.Visibility = Visibility.Collapsed;

                AddFirstReferenceButton.Visibility = Visibility.Visible;

                return;
            }


            if (currentReferenceIndex >= CVSession.CurrentCV.References.Count)
            {
                currentReferenceIndex = CVSession.CurrentCV.References.Count - 1;
            }


            LoadCurrentReference();
        }





        // =========================================================
        // OPTIONAL COLLECTIONS
        // =========================================================

        private void InitializeOptionalCollections()
        {
            if (CVSession.CurrentCV.Projects == null)
            {
                CVSession.CurrentCV.Projects = new ObservableCollection<Project>();
            }

            if (CVSession.CurrentCV.Certificates == null)
            {
                CVSession.CurrentCV.Certificates = new ObservableCollection<Certificate>();
            }

            if (CVSession.CurrentCV.Achievements == null)
            {
                CVSession.CurrentCV.Achievements = new ObservableCollection<Achievement>();
            }


            if (CVSession.CurrentCV.References == null)
            {
                CVSession.CurrentCV.References = new ObservableCollection<Reference>();
            }
        }


        // =========================================================
        // WIZARD NAVIGATION
        // =========================================================
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentStep > 1)
            {
                currentStep--;

                ShowStep(currentStep);
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            SaveCurrentStep();

            // 7 = Finish
            if (currentStep < 7)
            {
                currentStep++;

                ShowStep(currentStep);
            }
            else
            {
                SaveCV_Click(sender, e);
            }
        }


        // =========================================================
        // TEMPLATE
        // =========================================================

        private void SetInitialTemplate()
        {
            CVTemplateType template = CVSession.CurrentCV.SelectedTemplate;

            SelectTemplateComboBox(TemplateComboBox, template);

            ShowTemplate(template);


        }


        private void SelectTemplateComboBox(ComboBox comboBox, CVTemplateType template)
        {
            if (comboBox == null)
                return;

            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Tag == null)
                    continue;

                if (item.Tag.ToString() == template.ToString())
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
        }


        private void TemplateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TemplateComboBox == null)
                return;

            ComboBoxItem item = TemplateComboBox.SelectedItem as ComboBoxItem;

            if (item == null)
                return;

            if (item.Tag == null)
                return;

            CVTemplateType template;

            bool valid = Enum.TryParse(item.Tag.ToString(), out template);

            if (!valid)
                return;

            CVSession.CurrentCV.SelectedTemplate = template;

            CVSession.SelectedTemplate = template;

            ShowTemplate(template);

            if (SelectedTemplateText != null)
            {
                SelectedTemplateText.Text = template.ToString();
            }
        }


        private void ShowTemplate(CVTemplateType template)
        {
            UserControl selectedTemplate = null;

            switch (template)
            {
                case CVTemplateType.Modern:

                    selectedTemplate = new ModernTemplate();

                    break;

                case CVTemplateType.Classic:

                    selectedTemplate = new ClassicTemplate();

                    break;


                case CVTemplateType.Minimal:

                    selectedTemplate = new MinimalTemplate();

                    break;

                case CVTemplateType.Corporate:

                    selectedTemplate = new CorporateTemplate();

                    break;

                default:

                    selectedTemplate = new ModernTemplate();

                    break;
            }


            selectedTemplate.DataContext = CVSession.CurrentCV;

            TemplatePreview.Content = selectedTemplate;


            Dispatcher.BeginInvoke(new Action(() =>{UpdatePreviewPageSize(); FitTemplateToContent();}), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        private void FitTemplateToContent()
        {
            if (TemplatePreview == null)
                return;

            FrameworkElement template = TemplatePreview.Content as FrameworkElement;

            if (template == null)
                return;

            double width = template.Width;

            if (width <= 0)
                width = 760;

            template.Height = double.NaN;

            template.MinHeight = 0;

            template.MaxHeight = double.PositiveInfinity;

            template.Measure(new Size(width, double.PositiveInfinity));

            double requiredHeight = template.DesiredSize.Height;

            if (requiredHeight <= 0)
                return;

            template.Arrange(new Rect(0, 0, width, requiredHeight));

            template.UpdateLayout();
        }


        // PAGE 
        // =========================================================

        private void PageSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CVSession.CurrentCV == null)
                return;

            if (PageSizeComboBox.SelectedValue == null)
                return;

            try
            {
                CVPageSize selectedPageSize = (CVPageSize)PageSizeComboBox.SelectedValue;

                CVSession.CurrentCV.PageSize = selectedPageSize;

                UpdatePreviewPageSize();

                Dispatcher.BeginInvoke(new Action(() => { FitTemplateToContent(); }), DispatcherPriority.Loaded);
            }
            catch
            {
                // Ignore invalid selection during initialization.
            }
        }



        // =========================================================
        // PAGE SIZE PREVIEW
        // =========================================================

        private void UpdatePreviewPageSize()
        {
            if (TemplatePreview == null)
                return;

            FrameworkElement template = TemplatePreview.Content as FrameworkElement;

            if (template == null)
                return;

            if (CVSession.CurrentCV == null)
                return;

            CVPageSizeOption pageSize = CVPageSizeService.GetPageSizes().Find(x => x.Type == CVSession.CurrentCV.PageSize);

            if (pageSize == null)
                return;

            template.Width = pageSize.Width;

            template.Height = double.NaN;

            template.MinHeight = pageSize.Height;

            template.MaxHeight = double.PositiveInfinity;

            template.HorizontalAlignment = HorizontalAlignment.Left;

            template.VerticalAlignment = VerticalAlignment.Top;

            template.Measure(new Size(pageSize.Width, double.PositiveInfinity));

            template.Arrange(new Rect(0, 0, pageSize.Width, template.DesiredSize.Height));

            template.UpdateLayout();

        }


        // =========================================================
        // SAVE / PRINT CV
        // =========================================================

        private void SaveCV_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FrameworkElement cvVisual = TemplatePreview.Content as FrameworkElement;

                UpdatePreviewPageSize();

                FitTemplateToContent();

                cvVisual.UpdateLayout();

                double width = cvVisual.ActualWidth;

                double height = cvVisual.ActualHeight;

                if (width <= 0)
                    width = cvVisual.DesiredSize.Width;

                if (height <= 0)
                    height = cvVisual.DesiredSize.Height;

                if (width <= 0 ||
                    height <= 0)
                {
                    MessageBox.Show("The CV has no printable content.", "Save CV", MessageBoxButton.OK, MessageBoxImage.Warning);

                    return;
                }

                cvVisual.Measure(new Size(width, height));

                cvVisual.Arrange(new Rect(0, 0, width, height));

                cvVisual.UpdateLayout();

                System.Windows.Controls.PrintDialog
                    printDialog = new System.Windows.Controls.PrintDialog();

                bool? result = printDialog.ShowDialog();

                if (result != true)
                    return;

                printDialog.PrintVisual(cvVisual,"ResumeBuilder CV");

                MessageBox.Show("CV has been Saved.", "CV Saved", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Save CV Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }

}
