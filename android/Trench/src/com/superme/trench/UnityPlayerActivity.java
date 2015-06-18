package com.superme.trench;

import cn.jpush.android.api.JPushInterface;

/**
 * @deprecated Use UnityPlayerNativeActivity instead.
 */
public class UnityPlayerActivity extends UnityPlayerNativeActivity {
	@Override
	protected void onResume() {
		// TODO Auto-generated method stub
		super.onResume();
		JPushInterface.onResume(UnityPlayerActivity.this);
	}

	@Override
	protected void onPause() {
		// TODO Auto-generated method stub
		super.onStop();
		JPushInterface.onPause(UnityPlayerActivity.this);
	}
}