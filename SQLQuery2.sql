CREATE TABLE Habits
(
    ID int PRIMARY KEY,
    habit_type varchar(50),
    habit_name varchar(200),
);

CREATE TABLE HabitsValue
(
    HabitID int,
    habit_value varchar(500),
    habit_date varchar(50)
);