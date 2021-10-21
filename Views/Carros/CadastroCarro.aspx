<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CadastroCarro.aspx.cs" Inherits="EstaparCRUD.Views.Carros.CadastroCarro" %>

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
                        CADASTRO DE CARROS
                    </div>
                </div>
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <span class="col-2">Placa *:</span>
                    <div class="col-4">
                        <asp:TextBox ID="txtPlaca" MaxLength="8" runat="server" CssClass="form-control text-uppercase" placeholder="AAA0A000"></asp:TextBox>
                        <small class="busca">Esse campo é obrigatório!</small>
                    </div>
                    <div class="col-6"></div>
                </div>
                <div class="form-group row">
                    <span class="col-2">MARCA:</span>
                    <div class="col-4">
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlMarcas" OnInit="ddlMarcas_Init" OnSelectedIndexChanged="ddlMarcas_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    <span class="col-2">MODELO:</span>
                    <div class="col-4">
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlModelos">
                        </asp:DropDownList>
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
