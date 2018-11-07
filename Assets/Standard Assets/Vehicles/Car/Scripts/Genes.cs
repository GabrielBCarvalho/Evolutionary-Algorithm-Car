using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action{
	PressLeft,
	PressRight,
	Accelerate, 
	Brake,
	DoNothing
};

public struct Genes{
	Action left45Green;
	Action left45Red;

	Action frontGreen;
	Action frontRed;

	Action right45Green;
	Action right45Red;

	Action leftGreen;
	Action leftRed;

	Action rightGreen;
	Action rightRed;

	public Action Left45Green{
		get{ return left45Green; }
		set{ left45Green = value;}
	}

	public Action Left45Red{
		get{ return left45Red; }
		set{ left45Red = value;}
	}

	public Action FrontGreen{
		get{ return frontGreen; }
		set{ frontGreen = value;}
	}

	public Action FrontRed{
		get{ return frontRed; }
		set{ frontRed = value;}
	}

	public Action Right45Green{
		get{ return right45Green; }
		set{ right45Green = value;}
	}

	public Action Right45Red{
		get{ return right45Red; }
		set{ right45Red = value;}
	}

	public Action LeftGreen{
		get{ return leftGreen; }
		set{ leftGreen = value;}
	}

	public Action LeftRed{
		get{ return leftRed; }
		set{ leftRed = value;}
	}

	public Action RightGreen{
		get{ return rightGreen; }
		set{ rightGreen = value;}
	}

	public Action RightRed{
		get{ return rightRed; }
		set{ rightRed = value;}
	}
}
