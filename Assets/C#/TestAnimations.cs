using UnityEngine;
using System.Collections;

public class TestAnimations : MonoBehaviour 
{
	[SerializeField]
    private EnemyAnimatonComponent _animation;

    private void Start()
    {
        var temp = new Student("Valik"); 
        var temp1 = new Student("Olik"); 
        if(temp & temp1) Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", temp + temp1);       
    }

    private void OnGUI()
    {
        if(!_animation) return;

        if(GUI.Button(new Rect(10,10,100,50), "Run"))
        {
            _animation.RunAnimation();
        }

        if(GUI.Button(new Rect(10,60,100,50), "Attack"))
        {
             _animation.AttackAnimation();
        }

        if(GUI.Button(new Rect(10,110,100,50), "Die"))
        {
             _animation.DieAnimation();
        }

        if(GUI.Button(new Rect(10,160,100,50), "Hit"))
        {
            _animation.HitAnimation();
        }
    }
}


public class Student
{
    public string Name;

    public Student(string name)
    {
        Name = name;
    }

    public static bool operator &(Student std, Student std1)
    {
        return std != null && std1 != null;
    }

    public static bool operator |(Student std, Student std1)
    {
        return std != null || std1 != null;
    }

    public static bool operator true(Student std)
    {
        return std != null;
    }

    public static bool operator false(Student std)
    {
        return std == null;
    }

    public static string operator + (Student std, Student std1)
    {
        if(std != null && std1 != null) return std.Name + " " + std1.Name;
        return string.Empty;
    }
}