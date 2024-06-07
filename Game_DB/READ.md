## Made with Marmaid is down the page

``` mermaid
erDiagram
    USUARIO ||--o{ PARTIDA : jugador
    USUARIO ||--o{ SESION : sesiones
    PARTIDA ||--|{ CARTAS_EN_PARTIDA : partida
    NIVEL ||--|{ RECETA : nivel
    RECETA ||--|| CARTAS_EN_PARTIDA : receta
    CARTAS ||--|| CARTAS_EN_PARTIDA : carta

    USUARIO {
        int id_usuario PK
        int puntaje_maximo
        string usr_rank
        float average_dishes_per_event
        string username
        string password
    }

    PARTIDA {
        int id_partida PK
        date fecha
        int puntaje
        int id_usuario FK
    }

    SESION {
        int id_sesion PK
        string token
        date fecha_inicio
        date fecha_expiracion
        date ultima_actividad
        int id_usuario FK
    }

    CARTAS {
        int id_carta PK
        string nombre
        int valor_nutrimental
        string tipo
    }

    CARTAS_EN_PARTIDA {
        int id_carta_en_partida PK
        int id_partida FK
        int id_carta FK
        int id_receta FK
    }

    NIVEL {
        int id_nivel PK
        string titulo
    }

    RECETA {
        int id_receta PK
        bool es_principal
        int belongs_to_level FK
        json ingredientes
    }
```
