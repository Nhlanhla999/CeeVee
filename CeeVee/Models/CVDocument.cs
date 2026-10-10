using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Generic;
using CeeVee.Services;



namespace CeeVee.Models
{
    public class CVDocument : ObservableObject
    {
        private PersonalDetails _personalDetails;
        private Profile _profile;
        private ObservableCollection<Experience> _experiences;
        private ObservableCollection<Education> _educations;
        private ObservableCollection<Skill> _skills;
        private ObservableCollection<Language> _languages;
        private ObservableCollection<Project> _projects;
        private ObservableCollection<Certificate> _certificates;
        private ObservableCollection<Achievement> _achievements;
        private ObservableCollection<Reference> _references;
        private CVTemplateType _selectedTemplate;
        private CVPageSize _pageSize = CVPageSize.A4;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public CVDocument()
        {

            PersonalDetails = new PersonalDetails();

            Profile = new Profile();

            Experiences = new ObservableCollection<Experience>();

            Educations = new ObservableCollection<Education>();

            Skills = new ObservableCollection<Skill>();

            Languages = new ObservableCollection<Language>();

            Projects = new ObservableCollection<Project>();

            Certificates = new ObservableCollection<Certificate>();

            Achievements = new ObservableCollection<Achievement>();

            References = new ObservableCollection<Reference>();

        }

        public PersonalDetails PersonalDetails
        {
            get => _personalDetails;

            set
            {
                if (_personalDetails != null)
                {
                    _personalDetails.PropertyChanged -= PersonalDetails_PropertyChanged;
                }

                if (SetProperty(ref _personalDetails, value))
                {
                    if (_personalDetails != null)
                    {
                        _personalDetails.PropertyChanged += PersonalDetails_PropertyChanged;
                    }
                }
            }
        }


        public Profile Profile
        {
            get => _profile;

            set
            {
                if (_profile != null)
                {
                    _profile.PropertyChanged -= Profile_PropertyChanged;
                }

                if (SetProperty(ref _profile, value))
                {
                    if (_profile != null)
                    {
                        _profile.PropertyChanged += Profile_PropertyChanged;
                    }

                    OnPropertyChanged(nameof(HasProfile));
                }
            }
        }

        public ObservableCollection<Experience> Experiences
        {
            get => _experiences;

            set
            {
                UnsubscribeCollection(_experiences);

                if (SetProperty(ref _experiences, value))
                {
                    SubscribeCollection(_experiences);

                    OnPropertyChanged(nameof(HasExperiences));
                }
            }
        }


        public ObservableCollection<Education> Educations
        {
            get => _educations;

            set
            {
                UnsubscribeCollection(_educations);

                if (SetProperty(ref _educations, value))
                {
                    SubscribeCollection(_educations);

                    OnPropertyChanged(nameof(HasEducations));
                }
            }
        }

        public ObservableCollection<Skill> Skills
        {
            get => _skills;

            set
            {
                UnsubscribeCollection(_skills);

                if (SetProperty(ref _skills, value))
                {
                    SubscribeCollection(_skills);

                    OnPropertyChanged(nameof(HasSkills));
                }
            }
        }

        public ObservableCollection<Language> Languages
        {
            get => _languages;

            set
            {
                UnsubscribeCollection(_languages);

                if (SetProperty(ref _languages, value))
                {
                    SubscribeCollection(_languages);

                    OnPropertyChanged(nameof(HasLanguages));
                }
            }
        }



        public ObservableCollection<Project> Projects
        {
            get => _projects;

            set
            {
                UnsubscribeCollection(_projects);

                if (SetProperty(ref _projects, value))
                {
                    SubscribeCollection(_projects);

                    OnPropertyChanged(nameof(HasProjects));
                }
            }
        }


        public ObservableCollection<Certificate> Certificates
        {
            get => _certificates;

            set
            {
                UnsubscribeCollection(_certificates);

                if (SetProperty(ref _certificates, value))
                {
                    SubscribeCollection(_certificates);

                    OnPropertyChanged(nameof(HasCertificates));
                }
            }
        }


        public ObservableCollection<Achievement> Achievements
        {
            get => _achievements;

            set
            {
                UnsubscribeCollection(_achievements);

                if (SetProperty(ref _achievements, value))
                {
                    SubscribeCollection(_achievements);

                    OnPropertyChanged(nameof(HasAchievements));
                }
            }
        }

        public ObservableCollection<Reference> References
        {
            get => _references;

            set
            {
                UnsubscribeCollection(_references);

                if (SetProperty(ref _references, value))
                {
                    SubscribeCollection( _references);

                    OnPropertyChanged(nameof(HasReferences));
                }
            }
        }

        public CVTemplateType SelectedTemplate
        {
            get => _selectedTemplate;

            set => SetProperty(ref _selectedTemplate, value);
        }

        public CVPageSize PageSize
        {
            get => _pageSize;
            set
            {
                if (SetProperty(ref _pageSize, value))
                {
                    OnPropertyChanged(nameof(PageWidth));
                    OnPropertyChanged(nameof(PageHeight));
                }
            }
        }

        public double PageWidth
        {
            get
            {
                CVPageSizeOption option = CVPageSizeService.GetPageSizes().Find(x => x.Type == PageSize);

                return option != null ? option.Width : 793.7;
            }
        }

        public double PageHeight
        {
            get
            {
                CVPageSizeOption option = CVPageSizeService.GetPageSizes().Find(x => x.Type == PageSize);

                return option != null ? option.Height : 1122.5;
            }
        }

        public List<CVPageSizeOption> PageSizeOptions
        {
            get
            {
                return CVPageSizeService.GetPageSizes();
            }
        }


        // =========================================================
        // PROFILE PROPERTY CHANGES
        // =========================================================

        private void Profile_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Profile.Summary) || string.IsNullOrEmpty(e.PropertyName))
            {
                OnPropertyChanged(nameof(HasProfile));
            }
        }


        private void PersonalDetails_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // PersonalDetails does not currently control
            // section visibility.

            // Keeping this handler allows the CVDocument
            // to observe the object if more visibility
            // rules are added later.
        }

        // =========================================================
        // SECTION VISIBILITY
        // =========================================================

        public bool HasProfile
        {
            get
            {
                return Profile != null && !string.IsNullOrWhiteSpace(Profile.Summary);
            }
        }


        public bool HasExperiences
        {
            get
            {
                return CollectionHasContent(Experiences);
            }
        }


        public bool HasEducations
        {
            get
            {
                return CollectionHasContent(Educations);
            }
        }

        public bool HasSkills
        {
            get
            {
                return CollectionHasContent(Skills);
            }
        }


        public bool HasLanguages
        {
            get
            {
                return CollectionHasContent(Languages);
            }
        }




        public bool HasProjects
        {
            get
            {
                return CollectionHasContent(Projects);
            }
        }


        public bool HasCertificates
        {
            get
            {
                return CollectionHasContent(Certificates);
            }
        }


        public bool HasAchievements
        {
            get
            {
                return CollectionHasContent(Achievements);
            }
        }


        public bool HasReferences
        {
            get
            {
                return CollectionHasContent(References);
            }
        }

        // =========================================================
        // CHECK COLLECTION CONTENT
        // =========================================================

        private bool CollectionHasContent(IEnumerable collection)
        {
            if (collection == null)
                return false;


            foreach (object item in collection)
            {
                if (ItemHasContent(item))
                    return true;
            }


            return false;
        }


        private bool ItemHasContent(object item)
        {
            if (item == null)
                return false;


            PropertyInfo[] properties = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);


            foreach (PropertyInfo property
                     in properties)
            {
                if (!property.CanRead)
                    continue;


                object value;


                try
                {
                    value = property.GetValue(item, null);
                }
                catch
                {
                    continue;
                }


                if (value == null)
                    continue;


                // =================================================
                // STRING
                // =================================================

                if (value is string)
                {
                    if (!string.IsNullOrWhiteSpace(
                        value as string))
                    {
                        return true;
                    }

                    continue;
                }


                // =================================================
                // DATE
                // =================================================

                if (value is DateTime)
                {
                    return true;
                }


                // =================================================
                // BOOLEAN
                // =================================================

                if (value is bool)
                {
                    if ((bool)value)
                        return true;

                    continue;
                }
            }


            return false;
        }



        // =========================================================
        // COLLECTION SUBSCRIPTION
        // =========================================================

        private void SubscribeCollection(IEnumerable collection)
        {
            if (collection == null)
                return;


            INotifyCollectionChanged notifyCollection = collection as INotifyCollectionChanged;


            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged += Collection_CollectionChanged;
            }


            foreach (object item in collection)
            {
                SubscribeItem(item);
            }
        }


        private void UnsubscribeCollection(IEnumerable collection)
        {
            if (collection == null)
                return;


            INotifyCollectionChanged notifyCollection = collection as INotifyCollectionChanged;


            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged -= Collection_CollectionChanged;
            }


            foreach (object item in collection)
            {
                UnsubscribeItem(item);
            }
        }

        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (object item in e.OldItems)
                {
                    UnsubscribeItem(item);
                }
            }


            if (e.NewItems != null)
            {
                foreach (object item in e.NewItems)
                {
                    SubscribeItem(item);
                }
            }


            RefreshCollectionVisibility();
        }

        // =========================================================
        // ITEM PROPERTY CHANGES
        // =========================================================

        private void SubscribeItem(object item)
        {
            INotifyPropertyChanged notify = item as INotifyPropertyChanged;


            if (notify == null)
                return;


            notify.PropertyChanged -= Item_PropertyChanged;


            notify.PropertyChanged += Item_PropertyChanged;
        }

        private void UnsubscribeItem(object item)
        {
            INotifyPropertyChanged notify = item as INotifyPropertyChanged;

            if (notify == null)
                return;


            notify.PropertyChanged -= Item_PropertyChanged;
        }


        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RefreshCollectionVisibility();
        }


        // =========================================================
        // REFRESH VISIBILITY
        // =========================================================

        private void RefreshCollectionVisibility()
        {
            OnPropertyChanged(nameof(HasExperiences));

            OnPropertyChanged(nameof(HasEducations));

            OnPropertyChanged(nameof(HasSkills));

            OnPropertyChanged(nameof(HasLanguages));

            OnPropertyChanged(nameof(HasProjects));

            OnPropertyChanged(nameof(HasCertificates));

            OnPropertyChanged(nameof(HasAchievements));

            OnPropertyChanged(nameof(HasReferences));
        }

    }
}
