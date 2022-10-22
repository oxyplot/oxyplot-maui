
using ExampleLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SimpleDemo
{
    public partial class ExampleBrowser
    {
        public ExampleBrowser()
        {
            InitializeComponent();
        }

        private ExampleBrowserViewModel _viewModel;
        protected override void OnAppearing()
        {
            if (this.BindingContext != null)
                return;

            var examples = ExampleLibrary.Examples.GetList();

            _viewModel = new ExampleBrowserViewModel(examples.ToArray());
            this.BindingContext = _viewModel;
        }

        private async void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItemIndex < 0)
                return;

            var exampleInfo = e.SelectedItem as ExampleInfo;
            var page = new PlotViewPage
            {
                ExampleInfo = exampleInfo
            };
            await Navigation.PushAsync(page);

            LvExamples.SelectedItem = null;
        }
    }

    public class ExampleBrowserViewModel : ViewModelBase
    {
        private readonly ExampleInfo[] allExamples;
        public ExampleBrowserViewModel(ExampleInfo[] allExamples)
        {
            this.allExamples = allExamples;

            BuildGroup(allExamples);

            CreateCommands();
        }

        private readonly ObservableCollection<ExampleGroup> examples = new ObservableCollection<ExampleGroup>();
        public IReadOnlyList<ExampleGroup> Examples => examples;

        public ICommand FilterCommand { get; private set; }

        private string filterKey;
        public string FilterKey
        {
            get => filterKey;
            set => SetProperty(ref filterKey, value);
        }

        private void CreateCommands()
        {
            FilterCommand = new Command(Filter);
        }

        private void Filter()
        {
            if (string.IsNullOrWhiteSpace(FilterKey))
            {
                if (this.examples.Count == allExamples.Length)
                    return;

                this.examples.Clear();

                BuildGroup(allExamples);
                return;
            }

            var examples = allExamples.Where(x => IsStringMatch(x.Title, FilterKey) ||
                                     IsStringMatch(x.Category, FilterKey) ||
                                     x.Tags != null && x.Tags.Any(t => IsStringMatch(t, FilterKey)))
                 .ToList();
            this.examples.Clear();

            BuildGroup(examples);
        }

        private static bool IsStringMatch(string source, string key)
        {
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(key))
                return false;

            return source.IndexOf(key, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void BuildGroup(IEnumerable<ExampleInfo> examples)
        {
            this.examples.Clear();

            foreach (var g in examples.GroupBy(x => x.Category))
            {
                var group = new ExampleGroup(g.Key);
                foreach (var exampleInfo in g)
                {
                    group.Add(exampleInfo);
                }
                this.examples.Add(group);
            }
        }
    }

    public class ExampleGroup : ObservableCollection<ExampleInfo>
    {
        public string Category { get; set; }

        public string Subtitle { get; set; }

        public ExampleGroup(string category)
        {
            Category = category;
        }
    }
}
