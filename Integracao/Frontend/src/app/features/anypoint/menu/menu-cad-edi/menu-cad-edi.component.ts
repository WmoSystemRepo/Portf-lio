import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import {
  DynamicField,
  FormComponeteCrudComponent,
} from '../../../../shared/components/anypoint/form-crud/form-crud.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import Shepherd from 'shepherd.js';
import { MenuModel } from '../../../../shared/models/anypoint/menu.model';
import { MenuService } from '../../auth';

@Component({
  selector: 'app-menu-cad-edi',
  imports: [CommonModule, FormComponeteCrudComponent],
  templateUrl: './menu-cad-edi.component.html'
})
export class MenuCadEdiComponent implements OnInit {
  form!: FormGroup;
  isEditMode = false;
  id: string | null = null;
  registroSelecionado: any = null;
  campos: DynamicField[] = [];
  tour = new Shepherd.Tour({
    defaultStepOptions: {
      scrollTo: true,
      cancelIcon: { enabled: true },
      classes: 'shepherd-theme-arrows'
    },
    useModalOverlay: true
  });

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private service: MenuService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {

    this.id = this.route.snapshot.paramMap.get('idReferencia');
    this.isEditMode = !!this.id;
    this.setupTour();

    this.campos = [
      {
        key: 'id',
        label: 'ID do Menu',
        type: 'text',
        required: true,
        hidden: !this.isEditMode,
      },
      {
        key: 'nome',
        label: 'Nome do Menu',
        type: 'text',
        required: true,
      },
      {
        key: 'rota',
        label: 'O endereço da rota do Menu',
        type: 'text',
        required: true,
      },
      {
        key: 'icone',
        label: 'O Ícone do menu',
        type: 'text',
        required: true,
      },
      {
        key: 'ordenacaoMenu',
        label: 'Ordenação do Menu',
        type: 'number',
        required: true,
      },
      {
        key: 'ehMenuPrincipal',
        label: 'É um Menu-Principal',
        type: 'select',
        required: false,
        options: [
          { label: 'Sim', value: true },
          { label: 'Não', value: false },
        ],
      },
      {
        key: 'subMenuReferenciaPrincipal',
        label: 'Escolha o Menu Principal',
        type: 'select',
        required: false,
        loadOptions: () =>
          this.service
            .ObterListsMenusPrincipal()
            .toPromise()
            .then((x) => x ?? []),
        labelField: 'nome',
        valueField: 'id',
      },
      {
        key: 'dataCriacao',
        label: 'Data de Criação',
        type: 'text',
        required: false,
        hidden: !this.isEditMode,
      },
      {
        key: 'dataEdicao',
        label: 'Última Edição',
        type: 'text',
        required: false,
        hidden: !this.isEditMode,
      },
    ];

    this.buildForm();

    if (this.isEditMode) {
      this.service.ObterMenuPorId(this.id!).subscribe({
        next: (data: { [key: string]: any; }) => {
          this.registroSelecionado = data;
          this.buildForm();
          this.form.patchValue(data);
          this.cdr.detectChanges();
        },
        error: () => {
          Swal.fire('Erro', 'Erro ao carregar os dados do menu.', 'error');
          this.router.navigate(['/Menu/Lista']);
        },
      });
    }
  }

  buildForm(): void {
    const group: Record<string, any> = {};
    this.campos.forEach((field) => {
      group[field.key] = field.required ? ['', Validators.required] : [''];
    });
    this.form = this.fb.group(group);
  }

  salvarNovo(dados: MenuModel): void {

    const gerarDataAtual = (): string => new Date().toISOString();

    const menu: MenuModel = {
      nome: dados.nome,
      rota: dados.rota,
      icone: dados.icone,
      ordenacaoMenu: dados.ordenacaoMenu,
      ehMenuPrincipal: dados.ehMenuPrincipal,
      subMenuReferenciaPrincipal: dados.subMenuReferenciaPrincipal,
      dataCriacao: gerarDataAtual(),
    };

    this.service.RegistrarMenu(menu).subscribe({
      next: () => {
        Swal.fire('Criado', 'Menu criado com sucesso.', 'success');
        this.router.navigate(['/Menu/Lista']);
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao criar o menu.', 'error');
      },
    });
  }

  salvarEdicao(dados: MenuModel): void {

    const gerarDataAtual = (): string => new Date().toISOString();

    const menu: MenuModel = {
      id: this.id!,
      nome: dados.nome,
      rota: dados.rota,
      icone: dados.icone,
      ordenacaoMenu: dados.ordenacaoMenu,
      ehMenuPrincipal: dados.ehMenuPrincipal,
      subMenuReferenciaPrincipal: dados.subMenuReferenciaPrincipal ?? 0,
      dataCriacao: dados.dataCriacao,
      dataEdicao: gerarDataAtual(),
    };

    this.service.AtualizarMenu(this.id!, menu).subscribe({
      next: () => {
        Swal.fire('Atualizado', 'Menu atualizado com sucesso.', 'success');
        this.router.navigate(['/Menu/Lista']).then(() => {
          window.location.reload();
        });
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao atualizar o menu.', 'error');
      },
    });
  }

  cancelar(): void {
    this.router.navigate(['/Menu/Lista']);
  }

  setupTour(): void {
    this.tour = new Shepherd.Tour({
      defaultStepOptions: {
        scrollTo: true,
        cancelIcon: { enabled: true },
        classes: 'shadow-md bg-white',
      },
      useModalOverlay: true,
    });

    this.tour.addStep({
      id: 'form-title',
      title: 'Formulário de Menu',
      text: 'Utilize este formulário para cadastrar um novo Menu ou editar os dados de um já existente. Preencha cada campo com atenção às informações solicitadas.',
      attachTo: { element: '.app-container', on: 'right' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'campo-nome',
      title: 'Nome do Menu',
      text: 'Informe um nome para o Menu.',
      attachTo: { element: '#nome', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-rota',
      title: 'Rota do Menu',
      text: 'Defina o endereço da rota para esse Menu. Esse endereço da rota é a URL o qual o Menu se refere.',
      attachTo: { element: '#rota', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-icone',
      title: 'Ícone do Menu',
      text: 'Escolha um ícone para representar esse Menu. Por exemplo: "fas fa-home".',
      attachTo: { element: '#icone', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-ordenacaoMenu',
      title: 'Ordenação do Menu',
      text: 'Informe em qual posição na barra lateral seu menu será exibido.',
      attachTo: { element: '#ordenacaoMenu', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-menuPrincipal',
      title: 'É um Menu Principal',
      text: 'Aqui você decidira se o Menu é um Menu Principal. Se for um Menu Principal, ele poderá ter sub-Menus(Menu filhos).',
      attachTo: { element: '#ehMenuPrincipal', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-subMenuReferenciaPrincipal',
      title: 'Referência do Menu Principal',
      text: 'Selecione o Menu Principal para o Sub Menu que está sendo referência-se.',
      attachTo: { element: '#subMenuReferenciaPrincipal', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'acoes-formulario',
      title: 'Ações do Formulário',
      text: 'Após preencher todos os campos, clique em “Salvar” para concluir o cadastro ou em “Cancelar” para descartar as alterações.',
      attachTo: { element: '.button-group', on: 'top' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Finalizar', action: this.tour.complete },
      ],
    });
  }

  startTour(): void {
    this.tour.start();
  }
}