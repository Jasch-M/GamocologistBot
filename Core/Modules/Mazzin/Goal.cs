using System;
using Discord.Commands;

namespace Template.Modules.Mazzin;

public class Goal
{
    public Goal(string name, int value, bool isActive = false, bool isCompleted = false)
    {
        Name = name;
        Value = value;
        IsActive = isActive;
        IsCompleted = isCompleted;
    }

    public Goal(string goalStr)
    {
        Goal goal = Parse(goalStr);
        Name = goal.Name;
        Value = goal.Value;
        IsActive = goal.IsActive;
        IsCompleted = goal.IsCompleted;
    }

    public string Name { get; internal set; }
    
    public int Value { get; internal set; }
    
    public bool IsActive { get; internal set; }
    
    public bool IsCompleted { get; internal set; }

    public void Complete()
    {
        IsCompleted = true;
    }
    
    public void Activate()
    {
        IsActive = true;
    }
    
    public bool IsReached(int amount)
    {
        return amount >= Value;
    }

    public bool IsReached()
    {
        return IsActive;
    }

    public override string ToString()
    {
        return $"{Name}";
    }

    internal string ToStringFull()
    {
        string valueStr = Value.ToString();
        string activeStr = IsActive ? "Active" : "Inactive";
        string completedStr = IsCompleted ? "Completed" : "Incomplete";
        return $"{Name}, {valueStr}, {activeStr}, {completedStr}";
    }
    
    public override bool Equals(object obj)
    {
        return obj is Goal goal &&
               Name == goal.Name;
    }

    protected bool Equals(Goal other)
    {
        return Name == other.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
    
    public static bool operator ==(Goal goal1, Goal goal2)
    {
        return goal1.Equals(goal2);
    }
    
    public static bool operator !=(Goal goal1, Goal goal2)
    {
        return !(goal1 == goal2);
    }
    
    public static implicit operator bool(Goal goal)
    {
        return goal.IsActive;
    }
    
    public static implicit operator Goal(string name)
    {
        return new Goal(name, 0);
    }
    
    public static implicit operator Goal(int value)
    {
        return new Goal("", value);
    }

    public static bool operator <(Goal goal1, Goal goal2)
    {
        return goal1.Value < goal2.Value;
    }
    
    public static bool operator >(Goal goal1, Goal goal2)
    {
        return goal1.Value > goal2.Value;
    }
    
    public static bool operator <=(Goal goal1, Goal goal2)
    {
        return goal1.Value <= goal2.Value;
    }
    
    public static bool operator >=(Goal goal1, Goal goal2)
    {
        return goal1.Value >= goal2.Value;
    }

    internal static Goal Parse(string input)
    {
        string[] parts = input.Split(',');
        switch (parts.Length)
        {
            case 1:
            {
                string goalName = parts[0];
                return new Goal(goalName, 0);
            }
            case 2:
            {
                string goalName = parts[0];
                string goalValueString = parts[1];
                int goalValue = int.Parse(goalValueString);
                return new Goal(goalName, goalValue);
            }
            case 3:
            {
                string goalName = parts[0];
                string goalValueString = parts[1];
                string isActiveString = parts[2];
                int goalValue = int.Parse(goalValueString);
                bool isActive = bool.Parse(isActiveString);
                return new Goal(goalName, goalValue, isActive);
            }
            case 4:
            {
                string goalName = parts[0];
                string goalValueString = parts[1];
                string isActiveString = parts[2];
                string isCompletedString = parts[3];
                int goalValue = int.Parse(goalValueString);
                bool isActive = bool.Parse(isActiveString);
                bool isCompleted = bool.Parse(isCompletedString);
                return new Goal(goalName, goalValue, isActive, isCompleted);
            }
            default:
                throw new ArgumentException("Invalid goal format");
        }
    }
}