using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Common.Statics
{
	public static class StaticEmailSignature
	{
		public static string GetEmailSignature()
		{
			return $"<p>Petra Buron<br />"+
					$"Graf-von-Stauffenberg-Weg 5A<br />" +
					$"59379 Selm <br />" +
					$"Deutschland </p >" +
					$"<p>Tel.: 017684448594 <br />"+
					$"E-Mail: petra@marelibuDesign.de</p>" +
					$"<p>USt.wird nicht ausgewiesen, da der Verkäufer Kleinunternehmer im Sinne des UStG ist.</p>"+
					$"<p>Plattform der EU-Kommission zur Online-Streitbeilegung: <a href=\"https://ec.europa.eu/consumers/odr\">" +
					$" https://ec.europa.eu/consumers/odr " +
					$"</a></p>" +
					$"<p>Wir sind zur Teilnahme an einem Streitbeilegungsverfahren vor einer Verbraucherschlichtungsstelle weder verpflichtet noch bereit.</p>" +
					$"<p>Mitglied der Initiative<b>\"Fairness im Handel\".</b><br />" +
					$"Nähere Informationen:<a href=\"https://www.fairness-im-handel.de\" > https://www.fairness-im-handel.de</a></p>" +
					$"<p>Verantwortliche / r i.S.d. § 55 Abs. 2 RStV:<br />" +
					$"Petra Buron, Graf-von-Stauffenberg-Weg 5a, 59379 Selm</p>";
		}
	}
}
