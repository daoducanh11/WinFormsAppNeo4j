using Neo4jClient;

namespace WinFormsAppNeo4j
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var client = new BoltGraphClient("bolt://localhost:7687", "neo4j", "12345678");
            await client.ConnectAsync();

            var query = client.Cypher
                              .Match("(m:Movie)<-[r:ACTED_IN]-(b)")
                              .Return((m, r) => new {
                                  Movie = m.As<Movie>(),
                                  Count = r.Count()
                              })
                              .OrderByDescending("Count")
                              .Limit(3); ;
            foreach (var result in await query.ResultsAsync)
                MessageBox.Show($"'{result.Movie.title}' had {result.Count} watchers");
        }
    }
}