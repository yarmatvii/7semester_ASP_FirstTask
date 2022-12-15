namespace _7semester_ASP_FirstTask.Models
{
	public class SortViewModel
	{
		public SortState SkinToneSort { get; } // значение для сортировки по имени
		public SortState AgeSort { get; }    // значение для сортировки по возрасту
		public SortState MasterSort { get; }
		public SortState Current { get; }     // текущее значение сортировки

		public SortViewModel(SortState sortOrder)
		{
			SkinToneSort = sortOrder == SortState.SkinToneAsc ? SortState.SkinToneDesc : SortState.SkinToneAsc;
			AgeSort = sortOrder == SortState.AgeAsc ? SortState.AgeDesc : SortState.AgeAsc;
			MasterSort = sortOrder == SortState.MasterAsc ? SortState.MasterDesc : SortState.MasterAsc;
			Current = sortOrder;
		}
	}
}
