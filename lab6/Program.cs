public interface ICombatState
{
    void ExecuteAction(CombatUnit unit);
    void UpdateState(CombatUnit unit);
}

public class CombatUnit
{
    private ICombatState _state;
    public string Name { get; }
    public int Health { get; set; } = 100;
    public int EnemyDistance { get; set; } = 50;
    public int EnemyHealth { get; set; } = 100;

    public CombatUnit(string name, ICombatState initialState)
    {
        Name = name;
        _state = initialState;
        Console.WriteLine($"[System]: {Name} initialized in {initialState.GetType().Name}.");
    }

    public void SetState(ICombatState state)
    {
        Console.WriteLine($"[State Change]: {Name} transitions to {state.GetType().Name}.");
        _state = state;
    }

    public void Update()
    {
        _state.ExecuteAction(this);
        _state.UpdateState(this);
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        Console.WriteLine($"[Event]: {Name} took {amount} damage! Current HP: {Health}");
    }
}

// --- Concrete States ---

public class AttackState : ICombatState
{
    public void ExecuteAction(CombatUnit unit)
    {
        Console.WriteLine($"-> {unit.Name} is attacking fiercely! (Deals 15 DMG)");
        unit.EnemyHealth -= 15;
    }

    public void UpdateState(CombatUnit unit)
    {
        if (unit.Health < 25)
            unit.SetState(new RetreatState());
        else if (unit.EnemyHealth < 30)
            unit.SetState(new PursuitState());
    }
}

public class DefenseState : ICombatState
{
    public void ExecuteAction(CombatUnit unit)
    {
        Console.WriteLine($"-> {unit.Name} is in defensive stance, regenerating slightly.");
        unit.Health += 5;
    }

    public void UpdateState(CombatUnit unit)
    {
        if (unit.EnemyDistance > 100)
            unit.SetState(new AttackState());
        else if (unit.Health < 15)
            unit.SetState(new RetreatState());
    }
}

public class RetreatState : ICombatState
{
    public void ExecuteAction(CombatUnit unit)
    {
        Console.WriteLine($"-> {unit.Name} is retreating to save their life!");
        unit.EnemyDistance += 20;
    }

    public void UpdateState(CombatUnit unit)
    {
        if (unit.Health > 50)
            unit.SetState(new DefenseState());
    }
}

public class PursuitState : ICombatState
{
    public void ExecuteAction(CombatUnit unit)
    {
        Console.WriteLine($"-> {unit.Name} is pursuing the weakened enemy!");
        unit.EnemyDistance -= 15;
    }

    public void UpdateState(CombatUnit unit)
    {
        if (unit.EnemyHealth <= 0)
        {
            Console.WriteLine($"[Victory]: {unit.Name} defeated the enemy!");
            unit.SetState(new DefenseState());
        }
        else if (unit.Health < 10)
            unit.SetState(new RetreatState());
    }
}

class Program
{
    static void Main()
    {
        var soldier = new CombatUnit("Infantryman", new AttackState());

        for (int i = 1; i <= 6; i++)
        {
            Console.WriteLine($"\n--- Round {i} ---");
            soldier.Update();

            if (i == 2)
                soldier.TakeDamage(80);
            if (i == 4)
                soldier.Health += 40;
        }
    }
}
