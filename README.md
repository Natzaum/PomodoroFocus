# PomodoroFocus

Uma aplicação multiplataforma de gerenciamento de produtividade baseada na Técnica Pomodoro, desenvolvida com .NET MAUI 9.0. Combine sessões de foco intenso com pausas estratégicas enquanto gerencia tarefas e desbloqueia conquistas.

## Características Principais

### Técnica Pomodoro
- Temporizador configurável para sessões de foco (padrão: 25 minutos)
- Pausas curtas (padrão: 5 minutos)
- Pausas longas (padrão: 15 minutos)
- Transições automáticas entre estados
- Visualização do progresso em tempo real

### Sistema de Tarefas
- Criar, editar e deletar tarefas
- Três estados: A Fazer, Em Progresso, Concluído
- Três níveis de prioridade: Baixa, Média, Alta
- Estatísticas em tempo real (total, pendentes, em progresso, concluídas)
- Contador de pomodoros por tarefa
- Datas de criação, conclusão e vencimento

### Sistema de Conquistas
- Desbloquear badges ao atingir marcos de produtividade
- Cinco níveis de conquista:
  - Primeiro Pomodoro (1 pomodoro completado)
  - Produtivo (10 pomodoros)
  - Mestre do Foco (50 pomodoros)
  - Guerreiro Incansável (100 pomodoros)
  - Lenda da Produtividade (250 pomodoros)
- Rastreamento persistente de progresso
- Notificações sonoras ao desbloquear conquistas

### Personalização Completa
- Temas: Claro, Escuro, Automático (segue configuração do sistema)
- Quatro esquemas de cores:
  - Energético (Vermelho)
  - Calmo (Azul)
  - Natural (Verde)
  - Neon (Purple)
- Sons customizáveis para eventos:
  - Padrão (clássico)
  - Suave (discreto)
  - Eletrônico (moderno)
  - Clássico (retrô)
  - Silencioso (sem som)
- Tipos de som configuráveis:
  - Notificação
  - Conquista
  - Clique
  - Pausa

### Notificações Sonoras Inteligentes
- Interrupção automática de áudio anterior ao tocar novo som
- Reprodução instantânea ao clicar múltiplas vezes
- Sincronização perfeita com ações do usuário

## Tecnologias Utilizadas

- **.NET MAUI 9.0**: Framework multiplataforma para UI
- **C# 13**: Linguagem de programação
- **XAML**: Markup para interfaces
- **SQLite**: Banco de dados local (via sqlite-net-pcl)
- **Dependency Injection**: Injeção de dependência nativa do MAUI
- **MVVM Binding**: Ligação de dados reativa

## Instalação e Configuração

### Clonar o Repositório
```bash
git clone https://github.com/Natzaum/PomodoroFocus.git
cd PomodoroFocus
```

### Compilar o Projeto
```bash
cd PomodoroFocus
dotnet build -f net9.0-windows10.0.19041.0
```

### Executar a Aplicação
```bash
dotnet run -f net9.0-windows10.0.19041.0
```

### Publicar para Windows
```bash
dotnet publish -f net9.0-windows10.0.19041.0 -c Release
```

## Serviços Disponíveis

### PomodoroService
Gerencia a lógica do temporizador Pomodoro

### TaskService
CRUD de tarefas com persistência em SQLite

### AchievementService
Gerencia conquistas e validação de condições

### ThemeService
Controla tema da aplicação (claro, escuro, automático)

### ColorSchemeService
Gerencia os esquemas de cores disponíveis

### SoundService
Reproduz efeitos sonoros com suporte a múltiplas plataformas

### SoundCustomizationService
Configura sons por tipo de evento

### SettingsService
Persiste configurações do usuário

## Banco de Dados

O projeto utiliza SQLite para persistência local de dados. O banco é criado automaticamente na primeira execução em:

**Windows**: `%APPDATA%\Local\pomodorofocus.db3`

**Android**: Diretório de dados privado da aplicação
