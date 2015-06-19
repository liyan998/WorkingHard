using UnityEngine;
using System.Collections;


public class RefactorData 
{
    private static RefactorData mInstrance;
    public static RefactorData Instance 
    {
        get
        {
            if (mInstrance == null)
            {
                mInstrance = new RefactorData();
            }
            return mInstrance;          
        }       

    }


    NetPlayer net_player;
    public NetPlayer NETPLAYER
    {
        get
        {
            if (net_player == null)
            {
                net_player = new NetPlayer();

            }
            return net_player;
        }
    }

    NetGameHelper mNetGameHelper = null;
    public NetGameHelper NetHelper
    {
        get { return mNetGameHelper; }
    }
}
