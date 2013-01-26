using UnityEngine;
using System.Collections.Generic;
using Prime31;


public class GameCenterGUIManager : MonoBehaviourGUI
{
#if UNITY_IPHONE
	// some useful ivars to hold information retrieved from GameCenter. These will make it easier
	// to test this code with your GameCenter enabled application because they allow the sample
	// to not hardcode any values for leaderboards and achievements.
	private List<GameCenterLeaderboard> leaderboards;
	private List<GameCenterAchievementMetadata> achievementMetadata;
	
	
	
	void Start()
	{
		// use anonymous delegates for this simple example for gathering data from GameCenter. In production you would want to
		// add and remove your event listeners in OnEnable/OnDisable
		GameCenterManager.categoriesLoaded += delegate( List<GameCenterLeaderboard> leaderboards )
		{
			this.leaderboards = leaderboards;
		};
		
		GameCenterManager.achievementMetadataLoaded += delegate( List<GameCenterAchievementMetadata> achievementMetadata )
		{
			this.achievementMetadata = achievementMetadata;
		};
		
		// after authenticating grab the players profile image
		GameCenterManager.playerAuthenticated += () =>
		{
			GameCenterBinding.loadProfilePhotoForLocalPlayer();
		};
		
		// always authenticate at every launch
		GameCenterBinding.authenticateLocalPlayer();
	}
	
	
	void OnGUI()
	{
		beginColumn();

		
		if( GUILayout.Button( "Load Achievement Metadata" ) )
		{
			GameCenterBinding.retrieveAchievementMetadata();
		}
		
		
		if( GUILayout.Button( "Get Raw Achievements" ) )
		{
			GameCenterBinding.getAchievements();
		}
		
		
		if( GUILayout.Button( "Post Achievement" ) )
		{
			if( achievementMetadata != null && achievementMetadata.Count > 0 )
			{
				int percentComplete = (int)Random.Range( 2, 60 );
				Debug.Log( "sending percentComplete: " + percentComplete );
				GameCenterBinding.reportAchievement( achievementMetadata[0].identifier, percentComplete );
			}
			else
			{
				Debug.Log( "you must load achievement metadata before you can post an achievement" );
			}
		}
		
		
		if( GUILayout.Button( "Issue Achievement Challenge" ) )
		{
			if( achievementMetadata != null && achievementMetadata.Count > 0 )
			{
				var playerIds = new string[] { "player1", "player2" };
				GameCenterBinding.issueAchievementChallenge( achievementMetadata[0].identifier, playerIds, "I challenge you" );
			}
			else
			{
				Debug.Log( "you must load achievement metadata before you can issue an achievement challenge" );
			}
		}
		
		
		if( GUILayout.Button( "Show Achievements" ) )
		{
			GameCenterBinding.showAchievements();
		}
		
		
		if( GUILayout.Button( "Reset Achievements" ) )
		{
			GameCenterBinding.resetAchievements();
		}
	
	
		endColumn( true );
		
		
		if( GUILayout.Button( "Get Player Alias" ) )
		{
			string alias = GameCenterBinding.playerAlias();
			Debug.Log( "Player alias: " + alias );
		}
		
		
		
		if( GUILayout.Button( "Load Leaderboard Data" ) )
		{
			GameCenterBinding.loadLeaderboardTitles();
		}
		
		
		if( GUILayout.Button( "Post Score" ) )
		{
			// We must have a leaderboard to post the score to
			if( leaderboards != null && leaderboards.Count > 0 )
			{
				Debug.Log( "about to report a random score for leaderboard: " + leaderboards[0].leaderboardId );
				GameCenterBinding.reportScore( Random.Range( 1, 99999 ), leaderboards[0].leaderboardId );
			}
		}
		
		
		if( GUILayout.Button( "Issue Score Challenge" ) )
		{
			// We must have a leaderboard to post the score to
			if( leaderboards != null && leaderboards.Count > 0 )
			{
				var playerIds = new string[] { "player1", "player2" };
				var score = Random.Range( 1, 9999 );
				GameCenterBinding.issueScoreChallenge( score, 0, leaderboards[0].leaderboardId, playerIds, "Beat this score!" );
			}
			else
			{
				Debug.Log( "you must load your leaderboards before you can issue a score challenge" );
			}
		}
		
		
		if( GUILayout.Button( "Show Leaderboards" ) )
		{
			GameCenterBinding.showLeaderboardWithTimeScope( GameCenterLeaderboardTimeScope.AllTime );
		}
		
		
		if( GUILayout.Button( "Get Raw Score Data" ) )
		{
			// We must have a leaderboard to retrieve scores from
			if( leaderboards != null && leaderboards.Count > 0 )
				GameCenterBinding.retrieveScores( false, GameCenterLeaderboardTimeScope.AllTime, 1, 25, leaderboards[0].leaderboardId );
			else
				Debug.Log( "Load leaderboard data before attempting to retrieve scores" );
		}

		
		if( GUILayout.Button( "Get Scores for Me" ) )
		{
			// We must have a leaderboard to retrieve scores from
			if( leaderboards != null && leaderboards.Count > 0 )
				GameCenterBinding.retrieveScoresForPlayerId( GameCenterBinding.playerIdentifier(), leaderboards[0].leaderboardId );
			else
				Debug.Log( "Load leaderboard data before attempting to retrieve scores" );
		}
	
		
		if( GUILayout.Button( "Retrieve Friends" ) )
		{
			GameCenterBinding.retrieveFriends( true, false );
		}
		
		endColumn();
		
		
		if( bottomLeftButton( "Load Multiplayer Scene (Requires Multiplayer Plugin!)", 340 ) )
		{
			Application.LoadLevel( "GameCenterMultiplayerTestScene" );
		}
	}

#endif
}
