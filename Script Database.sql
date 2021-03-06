USE [Nutribuddy]
GO
/****** Object:  Table [dbo].[Accion]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDPadre] [int] NULL,
	[Nombre] [nvarchar](300) NOT NULL,
	[Activo] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Centro]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Centro](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDUsuarioAdminitrador] [int] NULL,
	[IDPlan] [int] NOT NULL,
	[CIF] [nvarchar](9) NOT NULL,
	[NombreCentro] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](max) NOT NULL,
	[Telefono] [nvarchar](12) NOT NULL,
	[Direccion] [nvarchar](250) NULL,
	[IDProvincia] [int] NOT NULL,
	[Localidad] [nvarchar](100) NOT NULL,
	[CP] [nvarchar](5) NOT NULL,
	[FechaAlta] [datetime] NOT NULL,
	[FechaBaja] [datetime] NULL,
	[MotivoBaja] [nvarchar](500) NULL,
	[Activo] [bit] NOT NULL,
	[EsPresencial] [bit] NOT NULL,
	[EsOnline] [bit] NOT NULL,
	[Website] [nvarchar](300) NULL,
	[Email] [nvarchar](150) NOT NULL,
	[MaxTrabajadores] [int] NOT NULL,
	[TrabajadoresActuales] [int] NOT NULL,
 CONSTRAINT [PK_Centro] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CentroTrabajador]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CentroTrabajador](
	[IDTrabajador] [int] NOT NULL,
	[IDCentro] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_CentroTrabajador] PRIMARY KEY CLUSTERED 
(
	[IDTrabajador] ASC,
	[IDCentro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cita]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cita](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDTipo] [int] NOT NULL,
	[IDTrabajador] [int] NOT NULL,
	[IDPaciente] [int] NOT NULL,
	[HoraInicio] [datetime] NOT NULL,
	[HoraFin] [datetime] NOT NULL,
	[Titulo] [nvarchar](60) NOT NULL,
	[Observaciones] [nvarchar](450) NULL,
	[Activo] [bit] NOT NULL,
	[FechaAlta] [datetime] NOT NULL,
	[IDCentro] [int] NOT NULL,
	[NotasPrivadas] [nvarchar](max) NULL,
	[PautasPaciente] [nvarchar](max) NULL,
	[Completada] [bit] NOT NULL,
 CONSTRAINT [PK_Cita] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comida]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comida](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDDiario] [int] NOT NULL,
	[IDTipoComida] [int] NOT NULL,
	[Alimentos] [nvarchar](350) NOT NULL,
	[Observaciones] [nvarchar](350) NOT NULL,
	[Activo] [bit] NOT NULL,
	[Dia] [datetime] NOT NULL,
 CONSTRAINT [PK_Comida] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ComidaTipo]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComidaTipo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](15) NOT NULL,
	[NombreEN] [nvarchar](15) NOT NULL,
 CONSTRAINT [PK_ComidaTipo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ComunidadAutonoma]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComunidadAutonoma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_ComunidadAutonoma] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contacto]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contacto](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](300) NOT NULL,
	[Nombre] [nvarchar](300) NOT NULL,
	[EsCentro] [bit] NOT NULL,
	[Telefono] [nvarchar](9) NULL,
	[Comentario] [nvarchar](500) NOT NULL,
	[FechaEnvio] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Deporte]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Deporte](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDDiario] [int] NOT NULL,
	[IDTipoDeporte] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_DeporteT] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeporteTipo]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeporteTipo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](15) NOT NULL,
	[NombreEN] [nvarchar](15) NOT NULL,
 CONSTRAINT [PK_Deporte] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Diario]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Diario](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDPaciente] [int] NOT NULL,
	[Dia] [datetime] NOT NULL,
	[AguaCantidad] [decimal](3, 2) NULL,
	[Medicamentos] [nvarchar](250) NULL,
	[Menstruaccion] [bit] NULL,
	[Activo] [bit] NOT NULL,
	[IDSuenyo] [int] NULL,
	[IDEstado] [int] NULL,
	[Deporte] [bit] NOT NULL,
	[ObsDeporte] [nvarchar](600) NULL,
	[IDMedidas] [int] NULL,
 CONSTRAINT [PK_Diario] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadoAnimicoDiario]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadoAnimicoDiario](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDDiario] [int] NOT NULL,
	[IDEstadoAnimico] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_EstadoAnimicoDiario] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadoAnimicoTipo]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadoAnimicoTipo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](15) NOT NULL,
	[NombreEN] [nvarchar](15) NOT NULL,
 CONSTRAINT [PK_EstadoAnimicoTipo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjetivoTipo]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjetivoTipo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](20) NULL,
	[NombreEN] [nvarchar](20) NULL,
 CONSTRAINT [PK_ObjetivoTipo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OpcionesDiarioTipo]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OpcionesDiarioTipo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](15) NOT NULL,
	[NombreEN] [nvarchar](15) NOT NULL,
 CONSTRAINT [PK_OpcionesDiarioTipo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Paciente]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Paciente](
	[IDUsuario] [int] NOT NULL,
	[DNI] [nvarchar](9) NOT NULL,
	[FechaNacimiento] [datetime] NOT NULL,
	[Direccion] [nvarchar](500) NOT NULL,
	[Telefono] [nvarchar](9) NOT NULL,
	[EsHombre] [bit] NOT NULL,
	[IDCentro] [int] NULL,
	[FormularioInicial] [bit] NOT NULL,
	[EsSedentario] [bit] NOT NULL,
	[IDObjetivo] [int] NULL,
	[BajaAutoestima] [bit] NOT NULL,
	[TieneEstres] [bit] NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Paciente] PRIMARY KEY CLUSTERED 
(
	[IDUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PacienteMedidas]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PacienteMedidas](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDPaciente] [int] NOT NULL,
	[Altura] [decimal](3, 0) NULL,
	[Peso] [decimal](5, 2) NULL,
	[CmPecho] [decimal](5, 2) NULL,
	[CmCintura] [decimal](5, 2) NULL,
	[CmCadera] [decimal](5, 2) NULL,
	[PGrasa] [decimal](5, 2) NULL,
	[PMagro] [decimal](5, 2) NULL,
	[PAgua] [decimal](5, 2) NULL,
	[PostedDate] [datetime] NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_PacienteMedidas] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Plan]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Plan](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Activo] [bit] NOT NULL,
	[IDPlanSucesor] [int] NULL,
	[MaxTrabajadores] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Provincia]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Provincia](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](250) NOT NULL,
	[IDCCAA] [int] NOT NULL,
 CONSTRAINT [PK_Provincias] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Puesto]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Puesto](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[colorAgenda] [nvarchar](7) NOT NULL,
 CONSTRAINT [PK_Especialidades] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PuestoTrabajador]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PuestoTrabajador](
	[IDUsuario] [int] NOT NULL,
	[IDPuesto] [int] NOT NULL,
	[IDCentro] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_PuestosTrabajador] PRIMARY KEY CLUSTERED 
(
	[IDUsuario] ASC,
	[IDPuesto] ASC,
	[IDCentro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rol]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rol_Accion]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol_Accion](
	[IDRol] [int] NOT NULL,
	[IDAccion] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IDRol] ASC,
	[IDAccion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trabajador]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trabajador](
	[IDUsuario] [int] NOT NULL,
	[NumSegSocial] [nvarchar](12) NOT NULL,
	[Activo] [bit] NOT NULL,
	[FechaAlta] [datetime] NOT NULL,
	[FechaBaja] [datetime] NULL,
	[DNI] [nvarchar](9) NOT NULL,
	[Nacionalidad] [nvarchar](60) NOT NULL,
	[Localidad] [nvarchar](100) NOT NULL,
	[Telefono] [nvarchar](9) NOT NULL,
 CONSTRAINT [PK_Trabajador_1] PRIMARY KEY CLUSTERED 
(
	[IDUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrabajadoresNB]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrabajadoresNB](
	[IDUsuario] [int] NOT NULL,
	[NumSegSocial] [nvarchar](12) NOT NULL,
	[Activo] [bit] NOT NULL,
	[FechaAlta] [datetime] NOT NULL,
	[FechaBaja] [datetime] NULL,
 CONSTRAINT [PK_TrabajadoresNB] PRIMARY KEY CLUSTERED 
(
	[IDUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 19/07/2022 22:55:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDRol] [int] NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[Password] [varbinary](max) NOT NULL,
	[Activo] [bit] NOT NULL,
	[FechaAlta] [datetime] NOT NULL,
	[FechaBaja] [datetime] NULL,
	[IDProvincia] [int] NOT NULL,
	[Nombre] [nvarchar](60) NULL,
	[Apellidos] [nvarchar](300) NULL,
	[CodigoRecuperacion] [nvarchar](6) NULL,
	[FechaCodigoRecuperacion] [datetime] NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Centro] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Centro] ADD  DEFAULT ((0)) FOR [EsPresencial]
GO
ALTER TABLE [dbo].[Centro] ADD  DEFAULT ((0)) FOR [EsOnline]
GO
ALTER TABLE [dbo].[Cita] ADD  DEFAULT ((0)) FOR [Completada]
GO
ALTER TABLE [dbo].[Diario] ADD  DEFAULT ((0)) FOR [Deporte]
GO
ALTER TABLE [dbo].[Paciente] ADD  DEFAULT ((0)) FOR [EsHombre]
GO
ALTER TABLE [dbo].[Paciente] ADD  DEFAULT ((0)) FOR [FormularioInicial]
GO
ALTER TABLE [dbo].[Paciente] ADD  DEFAULT ((0)) FOR [EsSedentario]
GO
ALTER TABLE [dbo].[Paciente] ADD  DEFAULT ((0)) FOR [BajaAutoestima]
GO
ALTER TABLE [dbo].[Paciente] ADD  DEFAULT ((0)) FOR [TieneEstres]
GO
ALTER TABLE [dbo].[Puesto] ADD  DEFAULT ('#000000') FOR [colorAgenda]
GO
ALTER TABLE [dbo].[Usuario] ADD  DEFAULT ((1)) FOR [IDProvincia]
GO
ALTER TABLE [dbo].[Accion]  WITH CHECK ADD  CONSTRAINT [FK_Acciones_Acciones] FOREIGN KEY([IDPadre])
REFERENCES [dbo].[Accion] ([ID])
GO
ALTER TABLE [dbo].[Accion] CHECK CONSTRAINT [FK_Acciones_Acciones]
GO
ALTER TABLE [dbo].[Centro]  WITH CHECK ADD  CONSTRAINT [FK_Centro_Plan] FOREIGN KEY([IDPlan])
REFERENCES [dbo].[Plan] ([ID])
GO
ALTER TABLE [dbo].[Centro] CHECK CONSTRAINT [FK_Centro_Plan]
GO
ALTER TABLE [dbo].[Centro]  WITH CHECK ADD  CONSTRAINT [FK_Centro_Provincia] FOREIGN KEY([IDProvincia])
REFERENCES [dbo].[Provincia] ([ID])
GO
ALTER TABLE [dbo].[Centro] CHECK CONSTRAINT [FK_Centro_Provincia]
GO
ALTER TABLE [dbo].[Centro]  WITH CHECK ADD  CONSTRAINT [FK_Centro_Usuario] FOREIGN KEY([IDUsuarioAdminitrador])
REFERENCES [dbo].[Usuario] ([ID])
ON UPDATE SET NULL
GO
ALTER TABLE [dbo].[Centro] CHECK CONSTRAINT [FK_Centro_Usuario]
GO
ALTER TABLE [dbo].[CentroTrabajador]  WITH CHECK ADD  CONSTRAINT [FK_CT_Centro] FOREIGN KEY([IDCentro])
REFERENCES [dbo].[Centro] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CentroTrabajador] CHECK CONSTRAINT [FK_CT_Centro]
GO
ALTER TABLE [dbo].[CentroTrabajador]  WITH CHECK ADD  CONSTRAINT [FK_CT_Trabajador] FOREIGN KEY([IDTrabajador])
REFERENCES [dbo].[Trabajador] ([IDUsuario])
GO
ALTER TABLE [dbo].[CentroTrabajador] CHECK CONSTRAINT [FK_CT_Trabajador]
GO
ALTER TABLE [dbo].[Cita]  WITH CHECK ADD  CONSTRAINT [FK_Cita_Centro] FOREIGN KEY([IDCentro])
REFERENCES [dbo].[Centro] ([ID])
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [FK_Cita_Centro]
GO
ALTER TABLE [dbo].[Cita]  WITH CHECK ADD  CONSTRAINT [FK_Cita_Paciente] FOREIGN KEY([IDPaciente])
REFERENCES [dbo].[Paciente] ([IDUsuario])
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [FK_Cita_Paciente]
GO
ALTER TABLE [dbo].[Cita]  WITH CHECK ADD  CONSTRAINT [FK_Cita_Puesto] FOREIGN KEY([IDTipo])
REFERENCES [dbo].[Puesto] ([ID])
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [FK_Cita_Puesto]
GO
ALTER TABLE [dbo].[Cita]  WITH CHECK ADD  CONSTRAINT [FK_Cita_TC] FOREIGN KEY([IDTrabajador])
REFERENCES [dbo].[Trabajador] ([IDUsuario])
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [FK_Cita_TC]
GO
ALTER TABLE [dbo].[Comida]  WITH CHECK ADD  CONSTRAINT [FK_Comida_Diario] FOREIGN KEY([IDDiario])
REFERENCES [dbo].[Diario] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comida] CHECK CONSTRAINT [FK_Comida_Diario]
GO
ALTER TABLE [dbo].[Comida]  WITH CHECK ADD  CONSTRAINT [FK_Comida_TipoComida] FOREIGN KEY([IDTipoComida])
REFERENCES [dbo].[ComidaTipo] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comida] CHECK CONSTRAINT [FK_Comida_TipoComida]
GO
ALTER TABLE [dbo].[Deporte]  WITH CHECK ADD  CONSTRAINT [FK_Deporte_Diario] FOREIGN KEY([IDDiario])
REFERENCES [dbo].[Diario] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Deporte] CHECK CONSTRAINT [FK_Deporte_Diario]
GO
ALTER TABLE [dbo].[Deporte]  WITH CHECK ADD  CONSTRAINT [FK_Deporte_DTipo] FOREIGN KEY([IDTipoDeporte])
REFERENCES [dbo].[DeporteTipo] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Deporte] CHECK CONSTRAINT [FK_Deporte_DTipo]
GO
ALTER TABLE [dbo].[Diario]  WITH CHECK ADD  CONSTRAINT [FK_Diario_Medidas] FOREIGN KEY([IDMedidas])
REFERENCES [dbo].[PacienteMedidas] ([ID])
GO
ALTER TABLE [dbo].[Diario] CHECK CONSTRAINT [FK_Diario_Medidas]
GO
ALTER TABLE [dbo].[Diario]  WITH CHECK ADD  CONSTRAINT [FK_Diario_OpDiario1] FOREIGN KEY([IDEstado])
REFERENCES [dbo].[OpcionesDiarioTipo] ([ID])
GO
ALTER TABLE [dbo].[Diario] CHECK CONSTRAINT [FK_Diario_OpDiario1]
GO
ALTER TABLE [dbo].[Diario]  WITH CHECK ADD  CONSTRAINT [FK_Diario_OpDiario2] FOREIGN KEY([IDSuenyo])
REFERENCES [dbo].[OpcionesDiarioTipo] ([ID])
GO
ALTER TABLE [dbo].[Diario] CHECK CONSTRAINT [FK_Diario_OpDiario2]
GO
ALTER TABLE [dbo].[Diario]  WITH CHECK ADD  CONSTRAINT [FK_Diario_Paciente] FOREIGN KEY([IDPaciente])
REFERENCES [dbo].[Paciente] ([IDUsuario])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Diario] CHECK CONSTRAINT [FK_Diario_Paciente]
GO
ALTER TABLE [dbo].[EstadoAnimicoDiario]  WITH CHECK ADD  CONSTRAINT [FK_EAD_Diario] FOREIGN KEY([IDDiario])
REFERENCES [dbo].[Diario] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EstadoAnimicoDiario] CHECK CONSTRAINT [FK_EAD_Diario]
GO
ALTER TABLE [dbo].[EstadoAnimicoDiario]  WITH CHECK ADD  CONSTRAINT [FK_EAD_EA] FOREIGN KEY([IDEstadoAnimico])
REFERENCES [dbo].[EstadoAnimicoTipo] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EstadoAnimicoDiario] CHECK CONSTRAINT [FK_EAD_EA]
GO
ALTER TABLE [dbo].[Paciente]  WITH CHECK ADD  CONSTRAINT [FK_Paciente_Centro] FOREIGN KEY([IDCentro])
REFERENCES [dbo].[Centro] ([ID])
GO
ALTER TABLE [dbo].[Paciente] CHECK CONSTRAINT [FK_Paciente_Centro]
GO
ALTER TABLE [dbo].[Paciente]  WITH CHECK ADD  CONSTRAINT [FK_Paciente_ObjetivoTipo] FOREIGN KEY([IDObjetivo])
REFERENCES [dbo].[ObjetivoTipo] ([ID])
GO
ALTER TABLE [dbo].[Paciente] CHECK CONSTRAINT [FK_Paciente_ObjetivoTipo]
GO
ALTER TABLE [dbo].[Paciente]  WITH CHECK ADD  CONSTRAINT [FK_Paciente_Usuario] FOREIGN KEY([IDUsuario])
REFERENCES [dbo].[Usuario] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Paciente] CHECK CONSTRAINT [FK_Paciente_Usuario]
GO
ALTER TABLE [dbo].[PacienteMedidas]  WITH CHECK ADD  CONSTRAINT [FK_Paciente_PacienteMedidas] FOREIGN KEY([IDPaciente])
REFERENCES [dbo].[Paciente] ([IDUsuario])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PacienteMedidas] CHECK CONSTRAINT [FK_Paciente_PacienteMedidas]
GO
ALTER TABLE [dbo].[Provincia]  WITH CHECK ADD  CONSTRAINT [FK_Provincias_ComunidadAutonoma] FOREIGN KEY([IDCCAA])
REFERENCES [dbo].[ComunidadAutonoma] ([ID])
GO
ALTER TABLE [dbo].[Provincia] CHECK CONSTRAINT [FK_Provincias_ComunidadAutonoma]
GO
ALTER TABLE [dbo].[PuestoTrabajador]  WITH CHECK ADD  CONSTRAINT [FK_PT_Centro] FOREIGN KEY([IDCentro])
REFERENCES [dbo].[Centro] ([ID])
GO
ALTER TABLE [dbo].[PuestoTrabajador] CHECK CONSTRAINT [FK_PT_Centro]
GO
ALTER TABLE [dbo].[PuestoTrabajador]  WITH CHECK ADD  CONSTRAINT [FK_PT_Puestos] FOREIGN KEY([IDPuesto])
REFERENCES [dbo].[Puesto] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PuestoTrabajador] CHECK CONSTRAINT [FK_PT_Puestos]
GO
ALTER TABLE [dbo].[PuestoTrabajador]  WITH CHECK ADD  CONSTRAINT [FK_PT_Usuario] FOREIGN KEY([IDUsuario])
REFERENCES [dbo].[Usuario] ([ID])
GO
ALTER TABLE [dbo].[PuestoTrabajador] CHECK CONSTRAINT [FK_PT_Usuario]
GO
ALTER TABLE [dbo].[Rol_Accion]  WITH CHECK ADD  CONSTRAINT [FK_RA_Accion] FOREIGN KEY([IDAccion])
REFERENCES [dbo].[Accion] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Rol_Accion] CHECK CONSTRAINT [FK_RA_Accion]
GO
ALTER TABLE [dbo].[Rol_Accion]  WITH CHECK ADD  CONSTRAINT [FK_RA_Rol] FOREIGN KEY([IDRol])
REFERENCES [dbo].[Rol] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Rol_Accion] CHECK CONSTRAINT [FK_RA_Rol]
GO
ALTER TABLE [dbo].[Trabajador]  WITH CHECK ADD  CONSTRAINT [FK_Trabajador_Usuario] FOREIGN KEY([IDUsuario])
REFERENCES [dbo].[Usuario] ([ID])
GO
ALTER TABLE [dbo].[Trabajador] CHECK CONSTRAINT [FK_Trabajador_Usuario]
GO
ALTER TABLE [dbo].[TrabajadoresNB]  WITH CHECK ADD  CONSTRAINT [FK_TrabajadoresNB_Usuario] FOREIGN KEY([IDUsuario])
REFERENCES [dbo].[Usuario] ([ID])
GO
ALTER TABLE [dbo].[TrabajadoresNB] CHECK CONSTRAINT [FK_TrabajadoresNB_Usuario]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Provincia] FOREIGN KEY([IDProvincia])
REFERENCES [dbo].[Provincia] ([ID])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Provincia]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY([IDRol])
REFERENCES [dbo].[Rol] ([ID])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Rol]
GO


--DATOS NECESARIOS PARA EJECUTAR LA APLICACIÓN
USE [Nutribuddy]
GO
SET IDENTITY_INSERT [dbo].[Rol] ON 

INSERT [dbo].[Rol] ([ID], [Nombre]) VALUES (1, N'SuperAdmin')
INSERT [dbo].[Rol] ([ID], [Nombre]) VALUES (2, N'Trabajador NB')
INSERT [dbo].[Rol] ([ID], [Nombre]) VALUES (3, N'Administrador Centro')
INSERT [dbo].[Rol] ([ID], [Nombre]) VALUES (4, N'Trabajador centro')
INSERT [dbo].[Rol] ([ID], [Nombre]) VALUES (5, N'Paciente')
SET IDENTITY_INSERT [dbo].[Rol] OFF
SET IDENTITY_INSERT [dbo].[Accion] ON 

INSERT [dbo].[Accion] ([ID], [IDPadre], [Nombre], [Activo]) VALUES (1, NULL, N'Mis datos', 1)
INSERT [dbo].[Accion] ([ID], [IDPadre], [Nombre], [Activo]) VALUES (4, 1, N'Perfil', 1)
INSERT [dbo].[Accion] ([ID], [IDPadre], [Nombre], [Activo]) VALUES (5, NULL, N'Agenda', 1)
INSERT [dbo].[Accion] ([ID], [IDPadre], [Nombre], [Activo]) VALUES (6, NULL, N'Centros', 1)
INSERT [dbo].[Accion] ([ID], [IDPadre], [Nombre], [Activo]) VALUES (7, NULL, N'Personal', 1)
INSERT [dbo].[Accion] ([ID], [IDPadre], [Nombre], [Activo]) VALUES (8, NULL, N'Pacientes', 1)
INSERT [dbo].[Accion] ([ID], [IDPadre], [Nombre], [Activo]) VALUES (9, NULL, N'Diario', 1)
INSERT [dbo].[Accion] ([ID], [IDPadre], [Nombre], [Activo]) VALUES (1002, 1, N'Mi centro', 1)
INSERT [dbo].[Accion] ([ID], [IDPadre], [Nombre], [Activo]) VALUES (1003, NULL, N'Progreso', 1)
SET IDENTITY_INSERT [dbo].[Accion] OFF
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (1, 6, 1)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (1, 7, 1)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (2, 5, 0)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (2, 6, 1)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (2, 7, 0)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (3, 1, 0)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (3, 4, 0)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (3, 5, 1)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (3, 7, 1)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (3, 8, 1)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (4, 1, 0)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (4, 4, 0)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (4, 5, 1)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (4, 7, 0)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (4, 8, 1)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (5, 1, 0)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (5, 4, 1)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (5, 9, 1)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (5, 1002, 1)
INSERT [dbo].[Rol_Accion] ([IDRol], [IDAccion], [Activo]) VALUES (5, 1003, 1)
SET IDENTITY_INSERT [dbo].[ComidaTipo] ON 

INSERT [dbo].[ComidaTipo] ([ID], [Nombre], [NombreEN]) VALUES (1, N'Desayuno', N'Breakfast')
INSERT [dbo].[ComidaTipo] ([ID], [Nombre], [NombreEN]) VALUES (2, N'Comida', N'Lunch')
INSERT [dbo].[ComidaTipo] ([ID], [Nombre], [NombreEN]) VALUES (3, N'Cena', N'Dinner')
INSERT [dbo].[ComidaTipo] ([ID], [Nombre], [NombreEN]) VALUES (4, N'Snack', N'Snack')
SET IDENTITY_INSERT [dbo].[ComidaTipo] OFF
SET IDENTITY_INSERT [dbo].[DeporteTipo] ON 

INSERT [dbo].[DeporteTipo] ([ID], [Nombre], [NombreEN]) VALUES (1, N'Correr', N'Running')
INSERT [dbo].[DeporteTipo] ([ID], [Nombre], [NombreEN]) VALUES (2, N'Ciclismo', N'Cycling')
INSERT [dbo].[DeporteTipo] ([ID], [Nombre], [NombreEN]) VALUES (3, N'Gimnasio', N'Gym')
INSERT [dbo].[DeporteTipo] ([ID], [Nombre], [NombreEN]) VALUES (4, N'Baile', N'Dancing')
INSERT [dbo].[DeporteTipo] ([ID], [Nombre], [NombreEN]) VALUES (5, N'Yoga', N'Yoga')
INSERT [dbo].[DeporteTipo] ([ID], [Nombre], [NombreEN]) VALUES (6, N'Natación', N'Swimming')
INSERT [dbo].[DeporteTipo] ([ID], [Nombre], [NombreEN]) VALUES (7, N'Boxeo', N'Boxing')
INSERT [dbo].[DeporteTipo] ([ID], [Nombre], [NombreEN]) VALUES (8, N'Fútbol', N'Football')
INSERT [dbo].[DeporteTipo] ([ID], [Nombre], [NombreEN]) VALUES (9, N'Artes marciales', N'Martial Arts')
INSERT [dbo].[DeporteTipo] ([ID], [Nombre], [NombreEN]) VALUES (10, N'Baloncesto', N'Basketball')
INSERT [dbo].[DeporteTipo] ([ID], [Nombre], [NombreEN]) VALUES (11, N'Caminar', N'Walk')
INSERT [dbo].[DeporteTipo] ([ID], [Nombre], [NombreEN]) VALUES (12, N'Pilates', N'Pilates')
INSERT [dbo].[DeporteTipo] ([ID], [Nombre], [NombreEN]) VALUES (13, N'Tenis', N'Tenis')
INSERT [dbo].[DeporteTipo] ([ID], [Nombre], [NombreEN]) VALUES (14, N'Spinning', N'Spinning')
INSERT [dbo].[DeporteTipo] ([ID], [Nombre], [NombreEN]) VALUES (15, N'Crossfit', N'Crossfit')
INSERT [dbo].[DeporteTipo] ([ID], [Nombre], [NombreEN]) VALUES (16, N'Otro', N'Other')
SET IDENTITY_INSERT [dbo].[DeporteTipo] OFF
SET IDENTITY_INSERT [dbo].[EstadoAnimicoTipo] ON 

INSERT [dbo].[EstadoAnimicoTipo] ([ID], [Nombre], [NombreEN]) VALUES (1, N'Contento', N'Happy')
INSERT [dbo].[EstadoAnimicoTipo] ([ID], [Nombre], [NombreEN]) VALUES (2, N'Activo', N'Active')
INSERT [dbo].[EstadoAnimicoTipo] ([ID], [Nombre], [NombreEN]) VALUES (3, N'Molesto', N'Upset')
INSERT [dbo].[EstadoAnimicoTipo] ([ID], [Nombre], [NombreEN]) VALUES (4, N'Enfadado', N'Angry')
INSERT [dbo].[EstadoAnimicoTipo] ([ID], [Nombre], [NombreEN]) VALUES (5, N'Preocupado', N'Worried')
INSERT [dbo].[EstadoAnimicoTipo] ([ID], [Nombre], [NombreEN]) VALUES (6, N'Pensativo', N'Thoughtful')
INSERT [dbo].[EstadoAnimicoTipo] ([ID], [Nombre], [NombreEN]) VALUES (7, N'Triste', N'Sad')
INSERT [dbo].[EstadoAnimicoTipo] ([ID], [Nombre], [NombreEN]) VALUES (8, N'Aburrido', N'Bored')
SET IDENTITY_INSERT [dbo].[EstadoAnimicoTipo] OFF
SET IDENTITY_INSERT [dbo].[ObjetivoTipo] ON 

INSERT [dbo].[ObjetivoTipo] ([ID], [Nombre], [NombreEN]) VALUES (1, N'Subir de peso', N'Gain weight')
INSERT [dbo].[ObjetivoTipo] ([ID], [Nombre], [NombreEN]) VALUES (2, N'Bajar de peso', N'Lose weight')
INSERT [dbo].[ObjetivoTipo] ([ID], [Nombre], [NombreEN]) VALUES (10, N'Mejorar mi salud', N'Improve my health')
SET IDENTITY_INSERT [dbo].[ObjetivoTipo] OFF
SET IDENTITY_INSERT [dbo].[OpcionesDiarioTipo] ON 

INSERT [dbo].[OpcionesDiarioTipo] ([ID], [Nombre], [NombreEN]) VALUES (1, N'Bien', N'Good')
INSERT [dbo].[OpcionesDiarioTipo] ([ID], [Nombre], [NombreEN]) VALUES (2, N'Regular', N'Normal')
INSERT [dbo].[OpcionesDiarioTipo] ([ID], [Nombre], [NombreEN]) VALUES (3, N'Mal', N'Bad')
SET IDENTITY_INSERT [dbo].[OpcionesDiarioTipo] OFF
SET IDENTITY_INSERT [dbo].[Plan] ON 

INSERT [dbo].[Plan] ([ID], [Nombre], [Activo], [IDPlanSucesor], [MaxTrabajadores]) VALUES (1, N'Lite', 1, NULL, 1)
INSERT [dbo].[Plan] ([ID], [Nombre], [Activo], [IDPlanSucesor], [MaxTrabajadores]) VALUES (2, N'Pyme', 1, NULL, 10)
INSERT [dbo].[Plan] ([ID], [Nombre], [Activo], [IDPlanSucesor], [MaxTrabajadores]) VALUES (4, N'Pro', 1, NULL, 100)
SET IDENTITY_INSERT [dbo].[Plan] OFF
SET IDENTITY_INSERT [dbo].[Provincia] ON 

INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (1, N'Álava', 17)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (2, N'Albacete', 8)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (3, N'Alicante', 18)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (4, N'Almería', 2)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (5, N'Asturias', 4)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (6, N'Ávila', 9)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (7, N'Badajoz', 11)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (8, N'Barcelona', 10)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (9, N'Burgos', 9)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (10, N'Cáceres', 11)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (11, N'Cádiz', 2)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (12, N'Cantabria', 7)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (13, N'Castellón', 18)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (14, N'Ciudad Real', 8)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (15, N'Córdoba', 2)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (16, N'A Coruña', 12)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (17, N'Cuenca', 8)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (18, N'Gerona', 10)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (19, N'Granada', 2)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (20, N'Guadalajara', 8)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (21, N'Guipúzcoa', 17)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (22, N'Huelva', 2)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (23, N'Huesca', 3)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (24, N'Baleares', 5)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (25, N'Jaén', 2)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (26, N'León', 9)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (27, N'Lérida', 10)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (28, N'Lugo', 12)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (29, N'Madrid', 14)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (30, N'Málaga', 2)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (31, N'Murcia', 15)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (32, N'Navarra', 16)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (33, N'Ourense', 12)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (34, N'Palencia', 9)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (35, N'La Palma', 6)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (36, N'Pontevedra', 12)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (37, N'La Rioja', 13)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (38, N'Salamanca', 9)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (39, N'Segovia', 9)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (40, N'Sevilla', 2)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (41, N'Soria', 9)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (42, N'Tarragona', 10)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (43, N'Tenerife', 6)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (44, N'Teruel', 3)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (45, N'Toledo', 8)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (46, N'Valencia', 18)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (47, N'Valladolid', 9)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (48, N'Vizcaya', 17)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (49, N'Zamora', 9)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (50, N'Zaragoza', 3)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (51, N'Ceuta', 19)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (52, N'Melilla', 20)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (53, N'La Gomera', 6)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (54, N'El Hierro', 6)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (55, N'Gran Canaria', 6)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (56, N'Fuerteventura', 6)
INSERT [dbo].[Provincia] ([ID], [Nombre], [IDCCAA]) VALUES (57, N'Lanzarote', 6)
SET IDENTITY_INSERT [dbo].[Provincia] OFF
SET IDENTITY_INSERT [dbo].[Puesto] ON 

INSERT [dbo].[Puesto] ([ID], [Nombre], [colorAgenda]) VALUES (1, N'Administrativo', N'#FFC300')
INSERT [dbo].[Puesto] ([ID], [Nombre], [colorAgenda]) VALUES (2, N'Nutricionista', N'#DAF7A6')
INSERT [dbo].[Puesto] ([ID], [Nombre], [colorAgenda]) VALUES (3, N'Endocrino', N'#FF5733')
INSERT [dbo].[Puesto] ([ID], [Nombre], [colorAgenda]) VALUES (4, N'Cirujano Plástico', N'#DB9AEB')
INSERT [dbo].[Puesto] ([ID], [Nombre], [colorAgenda]) VALUES (5, N'Psicólogo', N'#1AB3C2')
SET IDENTITY_INSERT [dbo].[Puesto] OFF
