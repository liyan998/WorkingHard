using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class xmlParticipation :BaseXmlReader<Dictionary<int,INFO_SHARE>,INFO_SHARE>
{
    protected override void OnRead (INFO_SHARE v)
	{
		m_Info.Add (v.Id, v);
	}
}

public class Participation : BaseXmlNode<Dictionary<int ,INFO_SHARE>,xmlParticipation>
{
	
}
