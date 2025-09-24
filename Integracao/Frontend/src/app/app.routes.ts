import { Routes } from '@angular/router';
import { AuthGuard } from './core/constants/guards/auth.guard';
import { DashboardComponent } from './features/anypoint/dashboard/dashboard.component';
import { PerfilComponent } from './features/anypoint/users/user-perfil/meu-perfil.component';
import { GestaoIntegracaoCadEdiComponent } from './features/anypoint/gestao-integracao/crud-formulario/gestao-integracao-cad-edi.component';
import { GestaoIntegracaoListaComponent } from './features/anypoint/gestao-integracao/crud-lista/gestao-integracao-lista.component';
import { UsuarioListaComponent } from './features/anypoint/users/user-list/usuario-lista.component';
import { UserFormComponent } from './features/anypoint/users/user-form/user-form.component';
import { RegraCadEdiComponent } from './features/anypoint/rules/regra-cad-edi/regra-card-edi.component';
import { GestaoMapearCamposListaComponent } from './features/anypoint/de-para/crud-lista/gestao-mapear-campos-lista-component';
import { GestaoMapearCamposFormComponent } from './features/anypoint/de-para/crud-formulario/gestao-mapear-campos-cred-edi-component';
import { DashboardDetalhadoComponent } from './features/anypoint/dashboard/detalhado/dashboard-detalhado.component';
import { DashboarVexpessesBimerdComponent } from './features/integracao/vexpesses-bimer/dashboard/dashboard-vexpenser_bimer.component';
import { RelatoriosComponent } from './features/integracao/vexpesses-bimer/relatorios/relatorios.component';
import { ListaTemplantesComponent } from './features/integracao/adobe-hub/tabela-preco-revenda/templantes-planinhas/lista-templantes/lista-templantes.component';
import { PlaninhasImportacaoViewComponent } from './features/integracao/adobe-hub/tabela-preco-revenda/importar-excel/planinhas-importacao-view/planinhas-importacao-view.component';
import { ListaArquivosImportadosComponent } from './features/integracao/adobe-hub/tabela-preco-revenda/importar-excel/lista-arquivo-excel-importados/lista-arquivo-excel-importados.component';
import { NovoTemplantesComponent } from './features/integracao/adobe-hub/tabela-preco-revenda/templantes-planinhas/novo-templates/novo-templantes.component';
import { NovoTemplantesViewComponent } from './features/integracao/adobe-hub/tabela-preco-revenda/templantes-planinhas/novo-templantes-view/novo-templantes-view.component';
import { RegistrarNovoTemplantesComponent } from './features/integracao/adobe-hub/tabela-preco-revenda/templantes-planinhas/registrar-novo-templantes/registrar-novo-templantes.component';
import { MontarExcelImportadosComponent } from './features/integracao/adobe-hub/tabela-preco-revenda/importar-excel/montar-excel-importados/montar-excel-importados.component';

import { LayoutComponent } from './layout/layout.component';
import { RegraListaComponent } from './features/anypoint/rules/regra-lista/regra-lista.component';
import { PermicoesListaComponent } from './features/anypoint/permission/permicoes-lista/permicoes-lista.component';
import { MenuListaComponent } from './features/anypoint/menu/menu-lista/menu-lista.component';
import { ForgotsenhaComponent } from './features/anypoint/auth/forrgot-senha/forgot-senha.component';
import { ConfigurationComponent } from './features/anypoint/config/configuracao.componente';
import { MenuCadEdiComponent } from './features/anypoint/menu/menu-cad-edi/menu-cad-edi.component';
import { PermicoesCadEdiComponent } from './features/anypoint/permission/Permicao-cad-edi/permicoes-cad-edi.component';
import { PlaninhasImportacaoComponent } from './features/integracao/adobe-hub/tabela-preco-revenda/importar-excel/planinhas-importacao/planinhas-importacao.component';
import { AdicionarProdutoExecelComponent } from './features/integracao/adobe-hub/tabela-preco-revenda/importar-excel/adicionar-produto-execel/adicionar-produto-execel.component';
import { DashboardBacemDolarComponent } from './features/integracao/bacen/dashboard/dashboard-bacem-dolar.component';
import { TypeTemplateListComponent } from './features/anypoint/typeTemplate/type-template-list/type-template-list.component';
import { TypeTemplateFormComponent } from './features/anypoint/typeTemplate/type-template-form/type-template-form.component';
import { MonitorSppBimerComponent } from './features/integracao/spp-bimer-invoce/componentes/monitor-geral/monitor-spp-bimer.component';
import { DeparaMensagensErroComponent } from './features/integracao/spp-bimer-invoce/componentes/depara-mensagens-erros/depara-mensagens-erro.component';
import { DocumentacaoTecnicaComponent } from './features/integracao/spp-bimer-invoce/componentes/documentacao-modulo/documentacao-tecnica.component/documentacao-tecnica.component';
import { ManualViewerComponent  } from './features/integracao/spp-bimer-invoce/componentes/documentacao-modulo/manual-usuario/manual-viewer.component';


export const routes: Routes = [
  //#region AUTH
  { path: 'login', redirectTo: 'auth', pathMatch: 'full' },
  {
    path: 'auth',
    loadComponent: () =>
      import('./features/anypoint/auth/Login/login.component').then(m => m.LoginComponent)
  },
  { path: 'esqueciMinhaSenha', component: ForgotsenhaComponent },
  //#endregion

  //#region ROTAS DO PROJETO
  {
    path: '',
    component: LayoutComponent,
    canActivate: [AuthGuard],
    canActivateChild: [AuthGuard],
    children: [

      //#region ANY-POINT
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'settings', component: ConfigurationComponent },
      { path: 'profile', component: PerfilComponent },


      { path: 'dashboard', component: DashboardComponent },
      { path: 'dashboard/detalhado', component: DashboardDetalhadoComponent },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

      //#region REGRA
      { path: 'Regras/Lista', component: RegraListaComponent },
      { path: 'Regras/Excluir/:id', component: RegraListaComponent },
      { path: 'Regras/Nova', component: RegraCadEdiComponent },
      { path: 'Regras/Editar/:id', component: RegraCadEdiComponent },
      //#endregion

      //#region GESTÃO DE INTEGRAÇÃO
      { path: 'GestaoIntegracoes/Lista', component: GestaoIntegracaoListaComponent },
      { path: 'GestaoIntegracoes/Excluir/:id', component: RegraListaComponent },
      { path: 'GestaoIntegracoes/Novo', component: GestaoIntegracaoCadEdiComponent },
      { path: 'GestaoIntegracoes/Editar/:id', component: GestaoIntegracaoCadEdiComponent },
      //#endregion

      //#region GESTÃO DE MAPEAR CAMPOS
      { path: 'GestaoMapearCampos/Lista', component: GestaoMapearCamposListaComponent },
      { path: 'GestaoMapearCampos/Novo', component: GestaoMapearCamposFormComponent },
      { path: 'GestaoMapearCampos/Editar/:idReferencia', component: GestaoMapearCamposFormComponent },
      //#endregion

      //#region USUARIO
      { path: 'Usuario/Lista', component: UsuarioListaComponent },
      { path: 'Usuario/Novo', component: UserFormComponent },
      { path: 'Usuario/Editar/:idReferencia', component: UserFormComponent },
      //#endregion

      //#region PERMISSÕES
      { path: 'Permissoes/Lista', component: PermicoesListaComponent },
      { path: 'Permissoes/Excluir/:id', component: PermicoesListaComponent },
      { path: 'Permissoes/Novo', component: PermicoesCadEdiComponent },
      { path: 'Permissoes/Editar/:id', component: PermicoesCadEdiComponent },
      //#endregion

      //#region MENUS
      { path: 'Menu/Lista', component: MenuListaComponent },
      { path: 'Menu/Novo', component: MenuCadEdiComponent },
      { path: 'Menu/Editar/:idReferencia', component: MenuCadEdiComponent },
      { path: 'Menu/Atualizar/:idReferencia', component: MenuCadEdiComponent },
      { path: 'Menu/Excluir/:idReferencia', component: MenuListaComponent },
      //#endregion

      //#region MENU REGRA
      { path: 'Menu/Regra/Lista', component: MenuListaComponent },
      { path: 'Menu/Regra/Novo', component: MenuListaComponent },
      { path: 'Menu/Regra/Editar/:idMenu', component: MenuListaComponent },
      { path: 'Menu/Regra/Excluir/:idMenu', component: MenuListaComponent },
      //#endregion

      //#region MENU PERMISSÃO
      { path: 'Menu/Permissao/Novo', component: MenuListaComponent },
      { path: 'Menu/Permissao/IdReferencia/:idReferencia', component: MenuListaComponent },
      { path: 'Menu/Permissao/IdMenu/:IdMenu', component: MenuListaComponent },
      { path: 'Menu/Permissao/Excluir/Referencia/:id', component: MenuListaComponent },
      //#endregion

      //#region MENU INTEGRAÇÃO
      { path: 'Menu/Integracao/Novo', component: MenuListaComponent },
      { path: 'Menu/Integracao/Excluir/:idReferencia', component: MenuListaComponent },
      { path: 'Menu/Integracao/IdMenu/:idMenu', component: MenuListaComponent },
      { path: 'Menu/Integracao/IdReferencia/:idReferencia', component: MenuListaComponent },
      { path: 'Menu/Integracao/Excluir/Referencia/:id', component: MenuListaComponent },
      //#endregion

      //#region TIPO TEMPLATE
      { path: 'Tipos/Templates/Lista', component: TypeTemplateListComponent },
      { path: 'Tipos/Templates/Novo', component: TypeTemplateFormComponent },
      { path: 'Tipos/Templates/Editar/:idReferencia', component: TypeTemplateFormComponent },
      { path: 'Tipos/Templates/Excluir/:idReferencia', component: TypeTemplateListComponent },
      //#endregion

      //#endregion

      //#region INTEGRAÇÃO VEXPESSES-BIMER
      { path: 'dashboard/vexpenses', component: DashboarVexpessesBimerdComponent },
      { path: 'Integracao/Vexpesses/Relatorios', component: RelatoriosComponent },
      //#endregion

      //#region INTEGRAÇÃO ADOBE_HUB

      //#region IMPORTAÇÂO TEMPLATE
      { path: 'Adobe/Templantes/Planinhas/Lista', component: ListaTemplantesComponent },
      { path: 'Adobe/Novo/Templantes', component: NovoTemplantesComponent },
      { path: 'Adobe/Novo/Templantes/View', component: NovoTemplantesViewComponent },
      { path: 'Adobe/Registrar/Novo/Templante', component: RegistrarNovoTemplantesComponent },
      //#endregion

      //#region IMPORTAÇÂO PLANILHA
      { path: 'Adobe/Planilhas/Importacao', component: PlaninhasImportacaoComponent },
      { path: 'Adobe/Planilhas/Visualizar/Importacao', component: PlaninhasImportacaoViewComponent },
      { path: 'Lista/Planilhas/Importadas', component: ListaArquivosImportadosComponent },
      { path: 'Montar/Planilha/Excel', component: MontarExcelImportadosComponent },
      { path: 'Execel/Adicionar/Produto', component: AdicionarProdutoExecelComponent },
      //#endregion

      //#endregion

      //#region INTEGRAÇÃO BACEM
      { path: 'Bacem/Dolar', component: DashboardBacemDolarComponent },
      //#endregion

      //#region INTEGRAÇÃO SPP BIMER INVOCE
      { path: 'Monitor/Spp/Bimer/Invoce', component: MonitorSppBimerComponent },
      { path: 'Monitor/Spp/Bimer/Invoce/Messagens/Erro', component: DeparaMensagensErroComponent },
      { path: 'Monitor/Spp/Bimer/Invoce/Documentacao/Tecnica', component: DocumentacaoTecnicaComponent },
      { path: 'Monitor/Spp/Bimer/Invoce/Manual/Usuario', component: ManualViewerComponent },
      //#endregion

      //#region FALLBACK GERAL
      { path: '**', redirectTo: 'dashboard' }
      //#endregion
    ]
  }
  //#endregion
];
