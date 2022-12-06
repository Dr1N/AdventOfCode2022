namespace AdventOfCode2022.Interfaces;

public interface ISolverFactory
{
    /// <summary>
    /// Return Solver for day
    /// </summary>
    /// <param name="day">Task day</param>
    /// <returns>Concrete Solver for day</returns>
    ISolver CreateSolver(int day);
}
