# Index 測試

db.Post.find({"Author": "Lee"}).explain("executionStats")

```
{
  explainVersion: '1',
  queryPlanner: {
    namespace: 'SocialNetworkDB.Post',
    indexFilterSet: false,
    parsedQuery: {
      Author: {
        '$eq': 'Lee'
      }
    },
    queryHash: '1328BDB4',
    planCacheKey: '1328BDB4',
    maxIndexedOrSolutionsReached: false,
    maxIndexedAndSolutionsReached: false,
    maxScansToExplodeReached: false,
    winningPlan: {
      stage: 'COLLSCAN',
      filter: {
        Author: {
          '$eq': 'Lee'
        }
      },
      direction: 'forward'
    },
    rejectedPlans: []
  },
  executionStats: {
    executionSuccess: true,
    nReturned: 199709,
    executionTimeMillis: 722,
    totalKeysExamined: 0,
    totalDocsExamined: 1000000,
    executionStages: {
      stage: 'COLLSCAN',
      filter: {
        Author: {
          '$eq': 'Lee'
        }
      },
      nReturned: 199709,
      executionTimeMillisEstimate: 58,
      works: 1000002,
      advanced: 199709,
      needTime: 800292,
      needYield: 0,
      saveState: 1000,
      restoreState: 1000,
      isEOF: 1,
      direction: 'forward',
      docsExamined: 1000000
    }
  },
  command: {
    find: 'Post',
    filter: {
      Author: 'Lee'
    },
    '$db': 'SocialNetworkDB'
  },
  serverInfo: {
    host: 'f3b4f10ce1ba',
    port: 27017,
    version: '6.0.5',
    gitVersion: 'c9a99c120371d4d4c52cbb15dac34a36ce8d3b1d'
  },
  serverParameters: {
    internalQueryFacetBufferSizeBytes: 104857600,
    internalQueryFacetMaxOutputDocSizeBytes: 104857600,
    internalLookupStageIntermediateDocumentMaxSizeBytes: 104857600,
    internalDocumentSourceGroupMaxMemoryBytes: 104857600,
    internalQueryMaxBlockingSortMemoryUsageBytes: 104857600,
    internalQueryProhibitBlockingMergeOnMongoS: 0,
    internalQueryMaxAddToSetBytes: 104857600,
    internalDocumentSourceSetWindowFieldsMaxMemoryBytes: 104857600
  },
  ok: 1
}
```
