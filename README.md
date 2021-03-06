# UCRSuite for Multivariate Template Matching with DTW & Top-k
Project that conveniently combines two UCRSuite implementations to apply multivariate, language-independent, and fast Template Matching under Dynamic Time Warping. 
This C#-Project extends mkarlesky's code (https://github.com/mkarlesky/UCRSuiteDTW) for finding the most similar subsequence by the top-k variant, according to wpleasant's R-library (https://github.com/wpleasant/dtwKnn). The original and theoretical work stems from the Keogh lab. (https://www.cs.ucr.edu/~eamonn/UCRsuite.html).
My project provides a handy merge between above projects and allows the (mostly) arbitrary choice of a programming language, as the querying code is surrounded by a light-weight Websocket server.  
