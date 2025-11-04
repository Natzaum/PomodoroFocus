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

## Arquitetura do Projeto

### Estrutura de Pastas

```
PomodoroFocus/
├── App.xaml / App.xaml.cs              # Aplicação principal
├── AppShell.xaml / AppShell.xaml.cs    # Shell e navegação
├── MainPage.xaml / MainPage.xaml.cs    # Página do Pomodoro
├── SettingsPage.xaml / SettingsPage.xaml.cs
├── TasksPage.xaml / TasksPage.xaml.cs
├── AchievementsPage.xaml / AchievementsPage.xaml.cs
├── MauiProgram.cs                      # Configuração DI
├── GlobalXmlns.cs                      # XML namespaces globais
├── Resources/                          # Recursos visuais
│   ├── Styles/
│   │   ├── Colors.xaml
│   │   └── Styles.xaml
│   ├── Fonts/
│   ├── Images/
│   └── Raw/                            # Arquivos de áudio
├── Platforms/                          # Código específico da plataforma
├── src/
│   ├── infrastructure/
│   │   ├── ColorResourceConverter.cs
│   │   ├── InverseBoolConverter.cs
│   │   ├── ServiceHelper.cs
│   │   ├── AchievementDebugHelper.cs
│   │   ├── BoolToColorConverter.cs
│   │   ├── BoolToOpacityConverter.cs
│   │   └── TaskConverters.cs
│   ├── models/
│   │   ├── Achievements.cs
│   │   ├── Settings.cs
│   │   └── Task.cs
│   ├── services/
│   │   ├── AchievementService.cs
│   │   ├── AchievementNotificationService.cs
│   │   ├── PomodoroService.cs
│   │   ├── SettingsService.cs
│   │   ├── SoundService.cs
│   │   ├── ThemeService.cs
│   │   ├── ColorSchemeService.cs
│   │   ├── SoundCustomizationService.cs
│   │   └── TaskService.cs
│   └── viewModels/
│       ├── BaseViewModel.cs
│       ├── HomeViewModel.cs
│       ├── SettingsViewModel.cs
│       ├── AchievementsViewModel.cs
│       └── TasksViewModel.cs
```

### Padrão de Arquitetura

O projeto utiliza o padrão MVVM (Model-View-ViewModel) com injeção de dependência:

- **Models**: Classes de dados (Task, Achievements, Settings)
- **Services**: Lógica de negócio e persistência (SQLite)
- **ViewModels**: Gerenciamento de estado e ligações de dados
- **Views (Pages)**: Interface do usuário em XAML

## Tecnologias Utilizadas

- **.NET MAUI 9.0**: Framework multiplataforma para UI
- **C# 13**: Linguagem de programação
- **XAML**: Markup para interfaces
- **SQLite**: Banco de dados local (via sqlite-net-pcl)
- **Dependency Injection**: Injeção de dependência nativa do MAUI
- **MVVM Binding**: Ligação de dados reativa

## Plataformas Suportadas

- Windows 10/11 (net9.0-windows10.0.19041.0)
- Android 9+ (net9.0-android)
- iOS (configurável)
- macOS Catalyst (configurável)

## Requisitos do Sistema

### Desenvolvimento
- .NET 9.0 SDK ou superior
- Visual Studio 2022 (17.9+) com carga de trabalho MAUI
- ou Visual Studio Code com extensões C# e MAUI

### Execução
- Windows 10/11 Build 19041 ou superior
- Android 9 ou superior
- 50MB de espaço em disco para instalação

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

## Uso da Aplicação

### Iniciar uma Sessão Pomodoro
1. Clique no botão "Iniciar" na página principal
2. O temporizador começará a contar regressivamente
3. Trabalhe durante toda a sessão
4. Ao terminar, uma pausa será iniciada automaticamente

### Gerenciar Tarefas
1. Navegue para a aba "Tarefas"
2. Digite o título da tarefa no campo de entrada
3. Selecione a prioridade (Baixa, Média, Alta)
4. Clique em "Adicionar"
5. Use os botões de ação:
   - Iniciar: Muda status para Em Progresso
   - Concluir: Muda status para Concluído
   - Resetar: Retorna para A Fazer
   - Deletar: Remove a tarefa

### Visualizar Conquistas
1. Navegue para a aba "Conquistas"
2. Veja o progresso de cada badge
3. Desfrute das notificações ao desbloquear novas conquistas

### Personalizar Configurações
1. Navegue para "Configurações"
2. Ajuste o tempo de foco (1-60 minutos)
3. Ajuste o tempo de pausa curta (1-60 minutos)
4. Ajuste o tempo de pausa longa (1-60 minutos)
5. Selecione o tema (Claro, Escuro, Automático)
6. Escolha um esquema de cores
7. Configure os sons para cada tipo de evento
8. Clique em "Salvar" para aplicar as mudanças

## Modelos de Dados

### Task
```csharp
public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public int PomodorCount { get; set; }
    public int CompletedPomodorCount { get; set; }
}
```

### Achievements
```csharp
public class Achievements
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ConditionKey { get; set; }
    public bool Unlocked { get; set; }
    public DateTime? UnlockedAt { get; set; }
}
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

## Conversores XAML Personalizados

- **InverseBoolConverter**: Inverte valores booleanos
- **ColorResourceConverter**: Converte strings em cores de recursos
- **BoolToColorConverter**: Converte booleano em cor
- **BoolToOpacityConverter**: Converte booleano em opacidade
- **StatusToVisibilityConverter**: Controla visibilidade baseado em status de tarefa

## Contribuindo

Contribuições são bem-vindas. Para contribuir:

1. Faça um Fork do projeto
2. Crie uma branch para sua feature (git checkout -b feature/NovaFeature)
3. Commit suas mudanças (git commit -m 'Adiciona NovaFeature')
4. Push para a branch (git push origin feature/NovaFeature)
5. Abra um Pull Request

## Licença

Este projeto está licenciado sob a Licença MIT - veja o arquivo LICENSE para detalhes.

## Autor

Desenvolvido por Natzaum

## Suporte

Para reportar bugs ou solicitar features, abra uma issue no repositório GitHub.

## Roadmap Futuro

- Integração com calendário para visualização de progresso
- Relatórios detalhados de produtividade
- Sincronização em nuvem de tarefas e progresso
- Modo foco com bloqueio de notificações
- Estatísticas por semana e mês
- Exportação de dados
- Integração com calendários externos
- Modo colaborativo para equipes

## Troubleshooting

### A aplicação não inicia
- Verifique se o .NET 9.0 SDK está instalado
- Limpe a pasta bin/obj: `dotnet clean && dotnet build`
- Recrie o banco de dados deletando o arquivo pomodorofocus.db3

### Sons não funcionam
- Verifique se os arquivos WAV estão em Resources/Raw/
- Confirme que as permissões de áudio estão ativadas
- No Windows, verifique o volume do sistema

### Tema não aplicando
- Recompile o projeto: `dotnet build --force`
- Reinicie a aplicação
- Verifique as cores definidas em Resources/Styles/Colors.xaml

### Tarefas não salvando
- Verifique se existe espaço em disco
- Confirme que a pasta de dados da aplicação existe
- Veja os logs de debug no console

## Changelog

### Versão 1.0.0
- Implementação completa do Pomodoro Timer
- Sistema de Tarefas com CRUD
- Sistema de Conquistas
- Temas claro/escuro/automático
- Quatro esquemas de cores
- Customização de sons por evento
- Suporte a múltiplas plataformas (Windows, Android)
- Persistência em SQLite
- UI responsiva com MVVM

## Agradecimentos

- .NET MAUI Team pelos excelentes frameworks
- Community por feedback e sugestões
- sqlite-net-pcl por persistência de dados robusta
