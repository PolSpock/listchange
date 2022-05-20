using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Listchange
{
	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// </summary>
	public partial class MyGame : Sandbox.Game
	{
		public MyGame()
		{
			if ( IsServer )
			{
				Log.Info( "My Gamemode Has Created Serverside!" );

				// Works
				DataString.Add( "I'm gonna update" );
				// Not works
				DataObject.Add( new MyObject() );
			}

			if ( IsClient )
			{
				Log.Info( "My Gamemode Has Created Clientside!" );

			}
		}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Not works

		[Net, Change( Name = "OnDataObjectChanged" )]
		public IList<MyObject> DataObject { get; set; }

		public void OnDataObjectChanged( IList<MyObject> oldValue, IList<MyObject> newValue )
		{
			Log.Info( "Will never changed :(" );

			Log.Info( oldValue );
			Log.Info( newValue );
		}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Works

		[Net, Change( Name = "OnDataStringChanged" )]
		public IList<string> DataString { get; set; }

		public void OnDataStringChanged( IList<string> oldValue, IList<string> newValue )
		{
			Log.Info( "Has changed" );

			Log.Info( oldValue );
			Log.Info( newValue );
		}



		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Basic stuff

		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			// Create a pawn for this client to play with
			var pawn = new Pawn();
			client.Pawn = pawn;

			// Get all of the spawnpoints
			var spawnpoints = Entity.All.OfType<SpawnPoint>();

			// chose a random one
			var randomSpawnPoint = spawnpoints.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

			// if it exists, place the pawn there
			if ( randomSpawnPoint != null )
			{
				var tx = randomSpawnPoint.Transform;
				tx.Position = tx.Position + Vector3.Up * 50.0f; // raise it up
				pawn.Transform = tx;
			}
		}
	}

}
