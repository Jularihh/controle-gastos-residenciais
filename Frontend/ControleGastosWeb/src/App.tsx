import { useEffect, useState } from "react";
import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5009/api",
});

function App() {
  const [pessoas, setPessoas] = useState<any[]>([]);
  const [totais, setTotais] = useState<any>(null);

  useEffect(() => {
    carregar();
  }, []);

  async function carregar() {
    try {
      const pessoas = await api.get("/Pessoas");
      setPessoas(pessoas.data);

      const resumo = await api.get("/Totais");
      setTotais(resumo.data);
    } catch (e) {
      console.log(e);
    }
  }

  return (
    <div style={{ padding: "30px", fontFamily: "Arial" }}>
      <h1>Controle de Gastos Residenciais</h1>

      <button onClick={carregar}>Atualizar</button>

      <h2>Pessoas</h2>

      <ul>
        {pessoas.map((p: any) => (
          <li key={p.id}>
            {p.nome} - {p.idade} anos
          </li>
        ))}
      </ul>

      <h2>Resumo Geral</h2>

      {totais && (
        <>
          <p>Receitas: R$ {totais.totalReceitas}</p>
          <p>Despesas: R$ {totais.totalDespesas}</p>
          <p>Saldo: R$ {totais.saldoTotal}</p>
        </>
      )}
    </div>
  );
}

export default App;