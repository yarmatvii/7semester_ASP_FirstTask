namespace _7semester_ASP_FirstTask.Models
{
	public class BagrainsViewModel
	{
		public IEnumerable<Slave> Slaves { get; }
		public PageViewModel PageViewModel { get; }
		public FilterViewModel FilterViewModel { get; }
		public SortViewModel SortViewModel { get; }
		public BagrainsViewModel(IEnumerable<Slave> slaves, PageViewModel pageViewModel,
			FilterViewModel filterViewModel, SortViewModel sortViewModel)
		{
			Slaves = slaves;
			PageViewModel = pageViewModel;
			FilterViewModel = filterViewModel;
			SortViewModel = sortViewModel;
		}
	}
}
