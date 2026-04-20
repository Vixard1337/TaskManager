# Diagrams

This file contains draft diagrams for project documentation.

## Class diagram (draft)

```mermaid
classDiagram
    class User {
        +string Id
        +string Name
        +string Surname
    }

    class TaskItem {
        +string Id
        +string Title
        +string Description
        +List~string~ Tags
        +bool IsDone
        +string UserId
    }

    User "1" --> "0..*" TaskItem : assigned tasks
```

## Use case diagram (draft)

```mermaid
flowchart LR
    U[User]

    UC1((Create user))
    UC2((List users))
    UC3((Create task))
    UC4((Edit task))
    UC5((Delete task))
    UC6((Mark task Done/Undo))
    UC7((Filter tasks by tag))
    UC8((List tasks))

    U --> UC1
    U --> UC2
    U --> UC3
    U --> UC4
    U --> UC5
    U --> UC6
    U --> UC7
    U --> UC8
```

> Note: These are documentation-ready drafts and can be exported to images for the final report.
