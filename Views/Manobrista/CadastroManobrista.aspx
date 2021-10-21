<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CadastroManobrista.aspx.cs" Inherits="EstaparCRUD.CadastroManobrista" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(window).on('load', function (event) {
            $('#preloader1').delay(0).fadeOut('slow', function () {
            });
        });
        $(function () {

            $(".btn-preloader").on('click', function (event) {
                $('#preloader1').delay(0).fadeIn('fast', function () {
                });
                $("#genCortina").fadeIn('fast');
            });
        });

    </script>
    <div class="card card-body">
        <div class="form-group row"></div>
        <div class="card card-default">
            <div class="card-header">
                <div class="row">
                    <div class="col-12 text-center">
                        CADASTRO DE MANOBRISTAS
                    </div>
                </div>
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <span class="col-2">Nome Completo *:</span>
                    <div class="col-10">
                        <asp:TextBox ID="txtNome" runat="server" CssClass="form-control text-uppercase" placeholder="Insira aqui o nome do manobrisa"></asp:TextBox>
                        <small class="busca">Esse campo é obrigatório!</small>
                    </div>
                </div>
                <div class="form-group row">
                    <span class="col-2">CPF*:</span>
                    <div class="col-4">
                        <asp:TextBox ID="txtCpf" runat="server" CssClass="form-control" placeholder="000.000.000-00" onkeyup="formataCPF(this,event);" MaxLength="15"></asp:TextBox>
                        <small class="busca">Esse campo é obrigatório!</small>
                   </div>
                    <span class="col-2">Data Nascimento*:</span>
                    <div class="col-4">
                        <asp:TextBox ID="txtNascimento" runat="server" CssClass="form-control" placeholder="DD/MM/AAAA" onkeyup="formataData(this,event);" MaxLength="10"></asp:TextBox>
                        <small class="busca">Esse campo é obrigatório!</small>
                   </div>
                </div>
                <div class="form-group row">
                    <div class="col text-center">
                        <asp:LinkButton ID="btnCadastrar" runat="server" OnClick="btnCadastrar_Click" CssClass="btn btn-success btn-preloader">
                            <i class="fas fa-save"></i> CADASTRAR
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="divCortina" runat="server" visible="false"></div>
</asp:Content>
