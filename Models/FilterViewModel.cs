using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace _7semester_ASP_FirstTask.Models
{
	public class FilterViewModel
	{
		public FilterViewModel(List<Master> masters, int master, string skinTone)
		{
			// устанавливаем начальный элемент, который позволит выбрать всех
			masters.Insert(0, new Master { Name = "All", Id = 0 });
			Masters = new SelectList(masters, "Id", "Name", master);
			SelectedMaster = master;
			SelectedSkinTone = skinTone;
		}
		public SelectList Masters { get; }
		public int SelectedMaster { get; }
		public string SelectedSkinTone { get; } // введенное имя
	}
}
